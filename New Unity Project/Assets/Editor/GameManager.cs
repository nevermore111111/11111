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


public class GameManager // ��Ӽ̳��� MonoBehaviour
{
    private static AnimationConfig gameConfig; // �� gameConfig ����Ϊ��̬�ֶ�

    [MenuItem("Assets/����json")] // ���˵�·����Ϊ�Լ���Ҫ��·��
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
        string json = JsonConvert.SerializeObject(gameConfig); // ʹ�� JsonConvert �������л�
        File.WriteAllText(filePath, json);

        Debug.Log("JSON data saved.");
    }

    [MenuItem("Assets/����json")] // ��Ӽ��� JSON �Ĳ˵���
    private static void LoadJson()
    {

        string filePath = Application.persistentDataPath + "/config.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            gameConfig = JsonConvert.DeserializeObject<AnimationConfig>(json); // ʹ�� JsonConvert ���з����л�
            Debug.Log($"{gameConfig.HitStrength[2][1]}");
            Debug.Log("JSON data loaded.");
        }
        else
        {
            Debug.Log("û������ļ�");
        }
    }
}
