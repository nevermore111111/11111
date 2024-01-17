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
public interface IGameConfigSave
{
    IGameConfigSave SaveDate();
}
public class AiBehavior : IGameConfigSave
{
    List<int> index;
    List<int[]> test;

    public IGameConfigSave SaveDate()
    {
        //通过reader得到当前这个类的数据，并且序列化之后储存起来
        //1先得到数据，对每一个字段进行赋值
        //序列化存储
        AiBehavior aiBehavior = AutoGetData.Fun01<AiBehavior>();
        Debug.Log(aiBehavior.index[0]);

        return this;
    }
    [MenuItem("Assets/测试方法")]
    public static void Save()
    {
        AiBehavior aiBehavior = AutoGetData.Fun01<AiBehavior>();
        Debug.Log(aiBehavior.index.Count);
    }
}
//对IGameConfigSave的自动进行赋值
public class AutoGetData
{
    public static T Fun01<T>() where T : IGameConfigSave, new()
    {
        T tar = new T();
        Type myType = tar.GetType();
        FieldInfo[] fields = myType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        for (int i = 0; i < fields.Length; i++)
        {
            // 获取字段的类型
            //这个字段一定是list<T>
            FieldInfo field = fields[i];
            string fieldName = field.Name;
            Type genericArgumentType = null;
            object fieldValue = null;

            if (field.FieldType.IsGenericType)//检查是否是通用泛型类
            {
                //泛型变量的泛型种类
                genericArgumentType = field.FieldType.GetGenericArguments()[0];
                Debug.Log(genericArgumentType);
            }
            else
            {
                Debug.LogError("非泛型,非list");
            }
            int workSheetNum = ExcelReaderHelper.GetWorkSheetNum(typeof(T).Name);
            if (genericArgumentType == typeof(int))
            {
                fieldValue = ExcelReaderHelper.ExcelReaderEZ<int>(workSheetNum, fieldName);
            }
            else if (genericArgumentType == typeof(float))
            {
                fieldValue = ExcelReaderHelper.ExcelReaderEZ<float>(workSheetNum, fieldName);
            }
            else if(genericArgumentType == typeof(string))
            {
                fieldValue = ExcelReaderHelper.ExcelReaderEZ<string>(workSheetNum, fieldName);
            }
            else if (genericArgumentType == typeof(int[]))
            {
                fieldValue = ExcelReaderHelper.ExcelReaderEZ<int[]>(workSheetNum, fieldName);
            }
            else if (genericArgumentType == typeof(float[]))
            {
                fieldValue = ExcelReaderHelper.ExcelReaderEZ<float[]>(workSheetNum, fieldName);
            }
            else if (genericArgumentType == typeof(string[]))
            {
                fieldValue = ExcelReaderHelper.ExcelReaderEZ<string[]>(workSheetNum, fieldName);
            }

            // 如果字段是值类型或者字符串，你可以设置一个默认值,这个方法返回一个list<T>

            // 设置字段值
            fields[i].SetValue(tar, fieldValue);
        }
        //这里写一个赋值的方法
        return tar;
    }
}
