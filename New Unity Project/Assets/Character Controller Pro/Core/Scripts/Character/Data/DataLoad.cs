using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System;

public class DataLoad : MonoBehaviour
{
    //这个类查看AutoGetData这个类中的方法，去在游戏运行前存储数据

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
        //string filePath = Application.persistentDataPath + "/config.json";

        FieldInfo[] fields = typeof(DataLoad).GetFields(BindingFlags.Public | BindingFlags.Instance);
        for (int i = 0; i < fields.Length; i++) 
        {
            FieldInfo field = fields[i];
            string fieldType = field.FieldType.ToString();
            string filePath = Path.Combine(Application.persistentDataPath, $"{fieldType}.json");
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                //为了加载速度，这里多写点
                switch(fieldType)
                {
                    case "AnimationConfig":
                        field.SetValue(Instance, JsonConvert.DeserializeObject<AnimationConfig>(json));
                        break;
                    case "AiBehavior":
                        field.SetValue(Instance, JsonConvert.DeserializeObject<AiBehavior>(json));
                        break;
                }
            }
            else
            {
                Debug.Log("没有这个文件");
            }
        }

       
    }
}
