using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.855f, 0.8623f, 0.87f)]
[TrackClipType(typeof(ArrayPlayableAsset))]
[TrackBindingType(typeof(GameObject))]
public class ArrayTrack : TrackAsset
{
   public int[] aaaa= new int[4] {1,1,1,1 };
}
