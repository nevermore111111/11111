using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackReceive : MonoBehaviour
{
    public CharacterInfo CharacterInfo;
    public ReceiveHitType receive = ReceiveHitType.Normal;



    public void Awake()
    {
        CharacterInfo = GetComponentInParent<CharacterInfo>();
        registerHit(receive);
    }
    // ����һ��ί�����ͣ����ڱ�ʾ������еķ���
    public delegate void HitHandler();

    // �����¼������ݲ�ͬ�Ļ�������ע�᲻ͬ�ķ���
    public event HitHandler OnHit;

    public void HitStart()
    {
        OnHit?.Invoke();
    }

    // ����ö�ٱ�ʾ��ͬ�Ļ�������
    public enum ReceiveHitType
    {
        Normal,
        Critical,
        ExtremeEvade
    }

    public void onNormalHit()
    {

    }
    public void onCriticalHit()
    {
        
    }
    public void onExtremeEvadeHit()
    {

    }

    // ���ջ��еķ��������ݻ������͵�����Ӧ���¼�
    public void registerHit(ReceiveHitType hitType)
    {
        switch (hitType)
        {
            case ReceiveHitType.Normal:
                // ������ͨ�����¼�
                OnHit += onNormalHit;
                break;

            case ReceiveHitType.Critical:
                // ���������¼�
                OnHit += onCriticalHit;
                break;

            case ReceiveHitType.ExtremeEvade:
                // �������������¼�
                OnHit += onExtremeEvadeHit;
                break;

            // ��������������͵Ĵ���

            default:
                Debug.LogError("Unsupported HitType: " + hitType);
                break;
        }
    }

    public bool isNormalReceive()
    {
        return receive == ReceiveHitType.Normal;
    }
    public bool isCriticalReceive() 
    {
        return receive == ReceiveHitType.Critical;
    }
    public bool isExtremeEvade() 
    {
        return receive == ReceiveHitType.ExtremeEvade; 
    }

}
