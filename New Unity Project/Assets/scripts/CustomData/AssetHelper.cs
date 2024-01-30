using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "AssetHelper", menuName = "CustomData/AssetHelper")]
public class AssetHelper : ScriptableObject
{
    public PlayableAsset[] AttackOnGround;
    public PlayableAsset[] AttackOnGround_fist;
    public PlayableAsset[] StartPlay;
    public PlayableAsset[] NormalMovement;
    public PlayableAsset[] AttackOffGround;
    public PlayableAsset[] Evade;
    public PlayableAsset[] All;
}

