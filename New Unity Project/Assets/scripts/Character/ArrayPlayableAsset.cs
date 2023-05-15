using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(ArrayPlayableAsset))]
public class ArrayPlayableAsset : PlayableAsset, ITimelineClipAsset
{
    public int[] arrayData;

    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ArrayPlayableBehaviour>.Create(graph);
        var playableBehaviour = playable.GetBehaviour();
        playableBehaviour.arrayData = arrayData;
        return playable;
    }
}
