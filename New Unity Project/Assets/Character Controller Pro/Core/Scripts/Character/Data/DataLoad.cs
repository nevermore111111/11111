using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DataLoad : MonoBehaviour
{
    public AnimationConfig animationConfig;

    private void Awake()
    {
        string filePath = Application.persistentDataPath + "/config.json";
        animationConfig = JsonConvert.DeserializeObject<AnimationConfig>(filePath);
    }
}
