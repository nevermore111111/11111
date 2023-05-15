using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class ArrayPlayableBehaviour : PlayableBehaviour
{
    public int[] arrayData;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        // 在这里处理数组数据
        Debug.Log("Array data length: " + arrayData.Length);
        foreach (int value in arrayData)
        {
            Debug.Log("Array value: " + value);
        }
    }
}
