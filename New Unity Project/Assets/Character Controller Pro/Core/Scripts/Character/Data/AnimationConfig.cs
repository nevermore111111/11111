using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Reflection;

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
public interface IGameConfigSave
{
    //这个接口只是一个标记作用，这个是需要存储的表格
}

public class AiBehavior : IGameConfigSave
{
    List<int> index;
    List<int[]> test;
  

//#if(UNITY_EDITOR)
//    public IGameConfigSave SaveDate()
//    {
//        //通过reader得到当前这个类的数据，并且序列化之后储存起来
//        //1先得到数据，对每一个字段进行赋值
//        //序列化存储
//        Debug.Log(Time.time);
//        AiBehavior aiBehavior =AutoGetData.Fun01<AiBehavior>();
//        Debug.Log(aiBehavior.index[0]);
//        Debug.Log(Time.time);
//        return this;
//    }
//#endif
//    //[MenuItem("Assets/测试方法")]
//    public static void Save()
//    {
//        Debug.Log(DateTime.Now);
//        AiBehavior aiBehavior = AutoGetData.Fun02<AiBehavior>();
//        Debug.Log(aiBehavior.index.Count);
//        Debug.Log(DateTime.Now);
//    }
}


//___________________________________________________________我是分隔线_________________________________________________________________________________
//___________________________________________________________我是分隔线_________________________________________________________________________________
//___________________________________________________________我是分隔线_________________________________________________________________________________
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
