using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SaveDuringPlay]
public class HitData : MonoBehaviour
{
    //编辑器类HitDataEditor
    //还需要声明一个时停的相关参数
    //包括1渐入渐出时间，2是持续时间，3是持续是时停倍数
    [Tooltip("如果不是-1，在编辑器模式下会每次攻击都强制使用这个类型的攻击（的震动和时停参数）")]
    public int ForceCurrentHit = -1;

    public int CurrentHit = -1;


    [Header("攻击类型00的时停参数")]
    public float fadeTime00 = 0.1f;
    public float stayTime00 = 0.05f;
    public float timeScale00 = 0.1f;


    [Header("攻击类型01的时停参数")]
    public float fadeTime01 = 0.1f;
    public float stayTime01 = 0.05f;
    public float timeScale01 = 0.1f;

    [Header("攻击类型02的时停参数")]
    public float fadeTime02 = 0.1f;
    public float stayTime02 = 0.05f;
    public float timeScale02 = 0.1f;

    [Header("攻击类型03的时停参数")]
    public float fadeTime03 = 0.1f;
    public float stayTime03 = 0.05f;
    public float timeScale03 = 0.1f;
    public float GetFadeTime(HitData hitData, int hitType)
    {
        switch (hitType)
        {
            case 0: return hitData.fadeTime00;
            case 1: return hitData.fadeTime01;
            case 2: return hitData.fadeTime02;
            case 3: return hitData.fadeTime03;
            default: throw new ArgumentOutOfRangeException(nameof(hitType));
        }
    }

    public float GetStayTime(HitData hitData, int hitType)
    {
        switch (hitType)
        {
            case 0: return hitData.stayTime00;
            case 1: return hitData.stayTime01;
            case 2: return hitData.stayTime02;
            case 3: return hitData.stayTime03;
            default: throw new ArgumentOutOfRangeException(nameof(hitType));
        }
    }

    public float GetTimeScale(HitData hitData, int hitType)
    {
        switch (hitType)
        {
            case 0: return hitData.timeScale00;
            case 1: return hitData.timeScale01;
            case 2: return hitData.timeScale02;
            case 3: return hitData.timeScale03;
            default: throw new ArgumentOutOfRangeException(nameof(hitType));
        }
    }
}

