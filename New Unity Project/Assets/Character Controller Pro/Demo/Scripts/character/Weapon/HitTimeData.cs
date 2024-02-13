using Cinemachine;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[SaveDuringPlay]
public class HitTimeData : MonoBehaviour
{
    [Header("本文件会在运行时保存")]
    //一个记录时停参数的类
    public int ForceCurrent = -1;
    [SerializeField]
    [ReadOnly]
    private int currentHit;
    public int CurrentHit
    {
        get
        {
            return currentHit;
        }
        set
        {
            if(Application.isEditor && ForceCurrent >= 0)
            {
                currentHit = ForceCurrent;
            }
            else
            {
                currentHit = value;
            }
            ChangeCurrentHitPara();
        }
    }

    public HitTimePara currentHitTimePara;
    public List<HitTimePara> hits;
    public void ChangeCurrentHitPara()
    {
        if (hits.Count > CurrentHit && CurrentHit >= 0)
        {
            currentHitTimePara = hits[CurrentHit];
        }
    }
    [Serializable]
    public class HitTimePara
    {
        [Range(0f, 0.2f)]
        public float fadeTime;
        [Range(0f, 0.4f)]
        public float stayTime;
        [Range(0f, 1f)]
        public float targetTimeScale;
    }

}
