using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// <summary>
public class ChangeClothes : MonoBehaviour
{
    int clothNum;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("clothNum"))
        {
            clothNum = PlayerPrefs.GetInt("clothNum");
        }
        else
        {
            clothNum = 0;
            PlayerPrefs.SetInt("clothNum", clothNum);
        }
    }
    public void NextCloth()
    {
        Debug.Log("nextCloth");
    }

    internal void ForCloth()
    {
        Debug.Log("forcloth");
    }
}
