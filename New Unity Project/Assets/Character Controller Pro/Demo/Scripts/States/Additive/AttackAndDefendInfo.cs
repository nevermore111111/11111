using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//攻击数据记录类
[System.Serializable]
public class AttackAndDefendInfo
{
    public AttackMode attackMode = AttackMode.AttackOnGround;
    public bool isAtttack;
    public int combo;
    public bool canInput;
    public bool isJustEnter;
    public bool canChangeState;
    public int maxCombo;
    public bool useGravity = false;
    public WeaponManager[] weaponManagers;

    public float defendStartTime = 0f;
    public int perfectDefendTime = 400;
    public DefendKind currentDenfendKind;

    public Action defendStartAction;
    public Action OnHit;
    public Action defendEndAction;

    public async void ChangeDefendFun()
    {
        float thisDefendStartTime = Time.time;
        await UniTask.Delay(perfectDefendTime);
        if (thisDefendStartTime == defendStartTime && currentDenfendKind == DefendKind.perfectDefend)
        {
            currentDenfendKind = DefendKind.normalDefend;
        }
        else
        {
            return;
        }
    }
}
public enum AttackMode
{
    AttackOnGround,
    //AttackOffGround,
    AttackOnGround_fist
}
public enum DefendKind
{
    perfectDefend,
    normalDefend,
    unDefend,//无防御
    OnlyDamege//霸体状态，只受伤害但是不打断
}