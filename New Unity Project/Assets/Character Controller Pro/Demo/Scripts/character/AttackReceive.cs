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
    // 定义一个委托类型，用于表示处理击中的方法
    public delegate void HitHandler();

    // 定义事件，根据不同的击中类型注册不同的方法
    public event HitHandler OnNormalHit;
    public event HitHandler OnCriticalHit;
    public event HitHandler OnExtremeEvadeHit;

    // 定义枚举表示不同的击中类型
    public enum ReceiveHitType
    {
        Normal,
        Critical,
        ExtremeEvade
    }

    // 接收击中的方法，根据击中类型调用相应的事件
    public void ReceiveHit(ReceiveHitType hitType)
    {
        switch (hitType)
        {
            case ReceiveHitType.Normal:
                // 触发普通击中事件
                OnNormalHit?.Invoke();
                break;

            case ReceiveHitType.Critical:
                // 触发暴击事件
                OnCriticalHit?.Invoke();
                break;

            case ReceiveHitType.ExtremeEvade:
                // 触发极限闪避事件
                OnExtremeEvadeHit?.Invoke();
                break;

            // 添加其他击中类型的处理

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
