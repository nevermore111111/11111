using UnityEngine;
using System.IO;
using UnityEditor;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using System;
using System.Linq;
using System.Reflection;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

[System.Serializable]


///这个类是用来存储数据的
public class GameDataStore // 添加继承自 MonoBehaviour
{
    private static AnimationConfig gameConfig; // 将 gameConfig 声明为静态字段

    [MenuItem("Assets/生成json")] // 将菜单路径改为自己想要的路径
    private static void GenerateJson()
    {
        gameConfig = new AnimationConfig
        {
            Index = ExcelReaderHelper.ExcelReaderEZ<int>(0, "Index"),
            ClipName = ExcelReaderHelper.ExcelReaderEZ<string>(0, "ClipName"),
            Combo = ExcelReaderHelper.ExcelReaderEZ<int>(0, "Combo"),
            AnmationStateName = ExcelReaderHelper.ExcelReaderEZ<string>(0, "AnmationStateName"),
            HitStrength = ExcelReaderHelper.ExcelReaderEZ<int[]>(0, "HitStrength"),
            HitDetect = ExcelReaderHelper.ExcelReaderEZ<string[]>(0, "HitDetect"),
            AnimStateInfo = ExcelReaderHelper.ExcelReaderEZ<int>(0, "AnimStateInfo"),
            SpAttackPar = ExcelReaderHelper.ExcelReaderEZ<float[]>(0, "SpAttackPar"),
            AttackDirection = ExcelReaderHelper.ExcelReaderEZ<float[]>(0, "AttackDirection"),
            HittedEffect = ExcelReaderHelper.ExcelReaderEZ<string[]>(0, "hittedEffect")
        };

        string filePath = Application.persistentDataPath + "/config.json";
        string json = JsonConvert.SerializeObject(gameConfig); // 使用 JsonConvert 进行序列化
        File.WriteAllText(filePath, json);

        Debug.Log("JSON data saved.");
    }

    [MenuItem("Assets/加载json")] // 添加加载 JSON 的菜单项
    private static void LoadJson()
    {

        string filePath = Application.persistentDataPath + "/config.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            gameConfig = JsonConvert.DeserializeObject<AnimationConfig>(json); // 使用 JsonConvert 进行反序列化
            Debug.Log($"{gameConfig.HitStrength[2][1]}");
            Debug.Log("JSON data loaded.");
        }
        else
        {
            Debug.Log("没有这个文件");
        }
    }
    //这个方法去读全部的表格，并且将表格中对应的类去赋值。
    private static void TestFun01()
    {

    }
}


public class AutoGetData
{

    //这个类写一个方法，反射dataload里面的所有变量。并且转成json存储起来
    //自己写的反射读表类，效率很低，不要在运行中使用！逻辑是开表-读一行-关表；循环后存储。读表不要超过1k行
    [MenuItem("Assets/存储表格")]
    private static void Fun01()
    {
        FieldInfo[] fields = typeof(DataLoad).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            //判断是不是静态变量，如果不是静态变量，就把他转化成xxx存储起来。
            //然后在dataload启动的时候，去加载每一个类
            Type type = field.FieldType;
            if (!type.IsStatic())
            {
                //非静态的就修改调用一个创建方法
                ConstructorInfo constructorInfo = type.GetConstructor(Type.EmptyTypes);
                object tar = null;
                if (constructorInfo != null)
                {
                    ////构建了第一个空的字段
                    //tar = constructorInfo.Invoke(null);
                }
                //我只需要调用根据type调用LoadDataReflection02
                MethodInfo targetMethod = typeof(AutoGetData).GetMethod("LoadDataReflection02");
                if (targetMethod != null)
                {
                    MethodInfo method = targetMethod.MakeGenericMethod(type);
                    AutoGetData auto = new AutoGetData();
                    //field.SetValue(tar, method.Invoke(auto, null));
                    tar = method.Invoke(auto, null);
                    SaveObject(tar);
                }
            }
        }
    }
    public static void SaveObject<T>(T obj)
    {
        string className = obj.GetType().ToString(); // 获取类名
        string filePath = Path.Combine(Application.persistentDataPath, $"{className}.json");

        try
        {
            string json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(filePath, json);
            Debug.Log($"Object of type {className} saved successfully at {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save object of type {className}: {e.Message}");
        }
    }


    /// <summary>
    /// 这个更快
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T LoadDataReflection02<T>() where T : IGameConfigSave, new()
    {
        T tar = new T();
        Type myType = tar.GetType();
        FieldInfo[] fields = myType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        int workSheetNum = ExcelReaderHelper.GetWorkSheetNum(typeof(T).Name);
        for (int i = 0; i < fields.Length; i++)
        {
            // 获取字段的类型
            //这个字段一定是list<T>
            FieldInfo field = fields[i];
            string fieldName = field.Name;
            //Type genericArgumentType;
            object fieldValue;

            if (field.FieldType.IsGenericType)//检查是否是通用泛型类
            {
                ////泛型变量的泛型种类
                //genericArgumentType = field.FieldType.GetGenericArguments()[0];
                //Debug.Log(genericArgumentType);
            }
            else
            {
                Debug.LogError("非泛型,非list");
            }
            fieldValue = ExcelReaderHelper.ExcelReaderEZ(workSheetNum, fieldName);
            // 如果字段是值类型或者字符串，你可以设置一个默认值,这个方法返回一个list<T>

            // 设置字段值
            fields[i].SetValue(tar, fieldValue);
        }
        //这里写一个赋值的方法
        return tar;
    }
}
