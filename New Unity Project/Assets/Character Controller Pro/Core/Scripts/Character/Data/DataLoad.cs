using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class DataLoad : MonoBehaviour
{
    public AnimationConfig animationConfig;
    public AiBehavior aiBehavior;

    private static DataLoad _instance;


    public static DataLoad Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DataLoad>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("DataLoad");
                    _instance = singletonObject.AddComponent<DataLoad>();
                }
            }
            return _instance;
        }
    }
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
