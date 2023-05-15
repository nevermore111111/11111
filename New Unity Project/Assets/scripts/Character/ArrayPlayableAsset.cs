using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(ArrayPlayableAsset))]
public class ArrayPlayableAsset : PlayableAsset, ITimelineClipAsset
{
    // �ڴ˴������������Ժͷ���...
    public int[] arrayData;
    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ArrayPlayableBehaviour>.Create(graph);
        var playableBehaviour = playable.GetBehaviour();
        playableBehaviour.arrayData = arrayData;
        // �ӹ�����л�ȡ�Ϸŵ� GameObject
        var trackBinding = owner.GetComponent<PlayableDirector>().GetGenericBinding(this) as GameObject;

        if (trackBinding != null)
        {
            // �ڴ˴������Ϸŵ� GameObject
            Debug.Log("Accessed GameObject: " + trackBinding.name);
            playableBehaviour.targetObject = trackBinding;
        }

        return playable;
    }
}
