using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationConfig
{
    public List<int> Index;
    public List<string> ClipName;
    public List<int> Combo;
    public List<string> AnmationStateName;
    public List<int[]> HitStrength;
    public List<string[]> HitDetect;
    public List<int> AnimStateInfo;
    // Add other data members as needed
}
public class SoloAnimaConfig
{
    public int Index;
    public string ClipName;
    public int Combo;
    public string AnmationStateName;
    public int[] HitStrength;
    public string[] HitDetect;
    public int AnimStateInfo;

    public SoloAnimaConfig(int index, string clipName, int combo, string animationStateName, int[] hitStrength, string[] hitDetect, int animStateInfo)
    {
        Index = index;
        ClipName = clipName;
        Combo = combo;
        AnmationStateName = animationStateName;
        HitStrength = hitStrength;
        HitDetect = hitDetect;
        AnimStateInfo = animStateInfo;
    }
}
