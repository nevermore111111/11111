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
    }
    // ����һ��ί�����ͣ����ڱ�ʾ������еķ���
    public delegate void HitHandler();

    // �����¼������ݲ�ͬ�Ļ�������ע�᲻ͬ�ķ���
    public event HitHandler OnNormalHit;
    public event HitHandler OnCriticalHit;
    public event HitHandler OnExtremeEvadeHit;

    // ����ö�ٱ�ʾ��ͬ�Ļ�������
    public enum ReceiveHitType
    {
        Normal,
        Critical,
        ExtremeEvade
    }

    // ���ջ��еķ��������ݻ������͵�����Ӧ���¼�
    public void ReceiveHit(ReceiveHitType hitType)
    {
        switch (hitType)
        {
            case ReceiveHitType.Normal:
                // ������ͨ�����¼�
                OnNormalHit?.Invoke();
                break;

            case ReceiveHitType.Critical:
                // ���������¼�
                OnCriticalHit?.Invoke();
                break;

            case ReceiveHitType.ExtremeEvade:
                // �������������¼�
                OnExtremeEvadeHit?.Invoke();
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
