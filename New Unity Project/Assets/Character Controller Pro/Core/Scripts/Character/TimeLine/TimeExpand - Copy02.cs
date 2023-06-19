using Cinemachine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.855f, 0.8623f, 0.87f)]
[TrackClipType(typeof(ArrayPlayableAsset02))]
[TrackBindingType(typeof(GameObject))]
public class CustomTrack02 : TrackAsset
{
    public List<float> floats = new List<float> { 0, 0, 0, 0 };
}

[TrackClipType(typeof(ArrayPlayableAsset02))]
public class ArrayPlayableAsset02 : PlayableAsset, ITimelineClipAsset
{
    public float[] arrayData;
   

    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ArrayPlayableBehaviour02>.Create(graph);
        var playableBehaviour = playable.GetBehaviour();

        // 从轨道绑定中获取拖放的 GameObject
        var trackBinding = owner.GetComponent<PlayableDirector>().gameObject;
       

        if (trackBinding != null)
        {
            // 在此处处理拖放的 GameObject
            //Debug.Log("Accessed GameObject: " + trackBinding.name);
           // playableBehaviour.targetObject = trackBinding;
        }

        // 传递数组数据给 PlayableBehaviour
        
        playableBehaviour.targetWeapon = trackBinding.GetComponentInChildren<WeaponManager>();
        playableBehaviour.arrayData = arrayData;

        return playable;
    }
}

[System.Serializable]
public class ArrayPlayableBehaviour02 : PlayableBehaviour
{
    public WeaponManager targetWeapon;
    public float[] arrayData;


    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        // 在这里可以使用访问到的 GameObject（targetObject）和数组数据进行操作


        // 在这里处理数组数据
        //Debug.Log("Array data length: " + arrayData.Length);
        //foreach (int value in arrayData)
        //{
        //    Debug.Log("Array value: " + value);
        //}
        if (arrayData.Length >= 3)
        {
            targetWeapon.impulsePar = arrayData;
        }
    }
}
