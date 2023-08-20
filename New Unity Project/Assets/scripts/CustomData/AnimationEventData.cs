using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "AnimationEventData", menuName = "CustomData/AnimationEventData")]
public class AnimationEventData : ScriptableObject
{
    //现在要传一个string 和一个 int，然后我
    public string[] AnmationStateName;
    public int[][] HitStrength;
    public string[][] Detection;
}

