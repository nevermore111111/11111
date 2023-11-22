using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所有攻击的参数
/// </summary>
public class AnimationConfig
{
    public List<int> Index;
    public List<string> ClipName;
    public List<int> Combo;
    public List<string> AnmationStateName;
    public List<int[]> HitStrength;
    public List<string[]> HitDetect;
    public List<int> AnimStateInfo;
    public List<float[]> SpAttackPar;
    public List<float[]> AttackDirection;
    public List<string[]> HittedEffect;

    // Add other data members as needed
}
/// <summary>
/// 单个攻击的参数
/// </summary>
public class SoloAnimaConfig
{
    public int Index;
    public string ClipName;
    public int Combo;
    public string AnmationStateName;
    public int[] HitStrength;
    public string[] HitDetect;
    public int AnimStateInfo;
    public float[] SpAttackPar;
    public float[] AttackDirection;
    public string[] HittedEffect;

    /// <summary>
    /// 用来记录单个攻击的参数
    /// </summary>
    /// <param name="index"></param>
    /// <param name="clipName"></param>
    /// <param name="combo"></param>
    /// <param name="animationStateName"></param>
    /// <param name="hitStrength"></param>
    /// <param name="hitDetect"></param>
    /// <param name="animStateInfo"></param>
    /// <param name="spAttackPar"></param>
    /// <param name="attackDirection"></param>
    public SoloAnimaConfig(int index, string clipName, int combo, string animationStateName, int[] hitStrength, string[] hitDetect, int animStateInfo, float[] spAttackPar, float[] attackDirection, string[] hittedEffect)
    {
        Index = index;
        ClipName = clipName;
        Combo = combo;
        AnmationStateName = animationStateName;
        HitStrength = hitStrength;
        HitDetect = hitDetect;
        AnimStateInfo = animStateInfo;
        SpAttackPar = spAttackPar;
        AttackDirection = attackDirection;
        HittedEffect = hittedEffect;
    }
}
