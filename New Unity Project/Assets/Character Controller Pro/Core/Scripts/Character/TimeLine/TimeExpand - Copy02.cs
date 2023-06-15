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

        // �ӹ�����л�ȡ�Ϸŵ� GameObject
        var trackBinding = owner.GetComponent<PlayableDirector>().gameObject;
       

        if (trackBinding != null)
        {
            // �ڴ˴������Ϸŵ� GameObject
            //Debug.Log("Accessed GameObject: " + trackBinding.name);
           // playableBehaviour.targetObject = trackBinding;
        }

        // �����������ݸ� PlayableBehaviour
        
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
        // ���������ʹ�÷��ʵ��� GameObject��targetObject�����������ݽ��в���


        // �����ﴦ����������
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
