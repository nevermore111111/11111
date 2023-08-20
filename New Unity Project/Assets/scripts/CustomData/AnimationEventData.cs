using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "AnimationEventData", menuName = "CustomData/AnimationEventData")]
public class AnimationEventData : ScriptableObject
{
    //����Ҫ��һ��string ��һ�� int��Ȼ����
    public string[] AnmationStateName;
    public int[][] HitStrength;
    public string[][] Detection;
}

