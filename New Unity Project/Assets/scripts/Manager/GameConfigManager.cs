using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameConfigManager : MonoBehaviour
{
    private static GameConfigManager _instance;


    public static GameConfigManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameConfigManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameConfigManager");
                    _instance = singletonObject.AddComponent<GameConfigManager>();
                }
            }
            return _instance;
        }
    }
}
