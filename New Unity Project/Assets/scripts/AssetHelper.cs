using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "AssetHelper", menuName = "AssetHelper")]
public class AssetHelper : ScriptableObject
{
    public PlayableAsset[] AttackOnGround;
    public PlayableAsset[] AttackOnGround_fist;
    public PlayableAsset[] AttackInAir;
    public PlayableAsset[] StartPlay;
}
