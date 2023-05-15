using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(ArrayPlayableAsset))]
public class ArrayPlayableAsset : PlayableAsset, ITimelineClipAsset
{
    // 在此处定义其他属性和方法...
    public int[] arrayData;
    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ArrayPlayableBehaviour>.Create(graph);
        var playableBehaviour = playable.GetBehaviour();
        playableBehaviour.arrayData = arrayData;
        // 从轨道绑定中获取拖放的 GameObject
        var trackBinding = owner.GetComponent<PlayableDirector>().GetGenericBinding(this) as GameObject;

        if (trackBinding != null)
        {
            // 在此处处理拖放的 GameObject
            Debug.Log("Accessed GameObject: " + trackBinding.name);
            playableBehaviour.targetObject = trackBinding;
        }

        return playable;
    }
}
