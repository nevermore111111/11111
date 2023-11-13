using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SaveDuringPlay]
public class HitData : MonoBehaviour
{
    //�༭����HitDataEditor
    //����Ҫ����һ��ʱͣ����ز���
    //����1���뽥��ʱ�䣬2�ǳ���ʱ�䣬3�ǳ�����ʱͣ����
    [Tooltip("�������-1���ڱ༭��ģʽ�»�ÿ�ι�����ǿ��ʹ��������͵Ĺ��������𶯺�ʱͣ������")]
    public int ForceCurrentHit = -1;

    public int CurrentHit = -1;


    [Header("��������00��ʱͣ����")]
    public float fadeTime00 = 0.1f;
    public float stayTime00 = 0.05f;
    public float timeScale00 = 0.1f;


    [Header("��������01��ʱͣ����")]
    public float fadeTime01 = 0.1f;
    public float stayTime01 = 0.05f;
    public float timeScale01 = 0.1f;

    [Header("��������02��ʱͣ����")]
    public float fadeTime02 = 0.1f;
    public float stayTime02 = 0.05f;
    public float timeScale02 = 0.1f;

    [Header("��������03��ʱͣ����")]
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

