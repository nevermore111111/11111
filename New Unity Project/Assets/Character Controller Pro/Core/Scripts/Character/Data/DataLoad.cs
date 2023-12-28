using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class DataLoad : MonoBehaviour
{
    public static AnimationConfig animationConfig;

    private void Awake()
    {
        string filePath = Application.persistentDataPath + "/config.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            animationConfig = JsonConvert.DeserializeObject<AnimationConfig>(json); // 使用 JsonConvert 进行反序列化
        }
        else
        {
            Debug.Log("没有这个文件");
        }
    }
}
