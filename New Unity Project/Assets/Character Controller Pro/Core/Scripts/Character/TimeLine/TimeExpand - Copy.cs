using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.855f, 0.8623f, 0.87f)]
[TrackClipType(typeof(ArrayPlayableAsset))]
[TrackBindingType(typeof(GameObject))]
public class CustomTrack : TrackAsset
{
}

[TrackClipType(typeof(ArrayPlayableAsset))]
public class ArrayPlayableAsset : PlayableAsset, ITimelineClipAsset
{
    public float[] arrayData;
    CinemachineFreeLook mainCinema;
    CameraEffects cameraEffects;

    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ArrayPlayableBehaviour>.Create(graph);
        var playableBehaviour = playable.GetBehaviour();

        // �ӹ�����л�ȡ�Ϸŵ� GameObject
        var trackBinding = owner.GetComponent<PlayableDirector>().GetGenericBinding(this) as GameObject;
        if(mainCinema == null)
        {
            mainCinema = owner.GetComponent<AnimatorFunction>().CinemachineFreeLook;
            cameraEffects = mainCinema.GetComponent<CameraEffects>();
        }

        if (trackBinding != null)
        {
            // �ڴ˴������Ϸŵ� GameObject
            Debug.Log("Accessed GameObject: " + trackBinding.name);
            playableBehaviour.targetObject = trackBinding;
        }

        // �����������ݸ� PlayableBehaviour
        playableBehaviour.arrayData = arrayData;
        playableBehaviour.cameraEffects = cameraEffects;


        return playable;
    }
}

[System.Serializable]
public class ArrayPlayableBehaviour : PlayableBehaviour
{
    public GameObject targetObject;
    public float[] arrayData;
    public GameObject tarobj;
    public CameraEffects cameraEffects;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        // ���������ʹ�÷��ʵ��� GameObject��targetObject�����������ݽ��в���


        // �����ﴦ����������
        //Debug.Log("Array data length: " + arrayData.Length);
        //foreach (int value in arrayData)
        //{
        //    Debug.Log("Array value: " + value);
        //}
        if(arrayData.Length>=3)
        {
            cameraEffects.shakeDuration = arrayData[0];
            cameraEffects.shakeAmplitude = arrayData[1];
            cameraEffects.shakeFrequency = arrayData[2];
        }
    }
}
