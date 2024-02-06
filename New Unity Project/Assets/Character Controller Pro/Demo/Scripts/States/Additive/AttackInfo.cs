using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//攻击数据记录类
[System.Serializable]
public class AttackInfo
{
    public AttackMode attackMode = AttackMode.AttackOnGround;
    public bool isAtttack;
    public int combo;
    public bool canInput;
    public bool isJustEnter;
    public bool canChangeState;
    public int maxCombo;
    public bool useGravity = false;
}
public enum AttackMode
{
    AttackOnGround,
    //AttackOffGround,
    AttackOnGround_fist
}
