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

[System.Serializable]


public class GameManager // 添加继承自 MonoBehaviour
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
}
