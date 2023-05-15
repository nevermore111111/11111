using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class ArrayPlayableBehaviour : PlayableBehaviour
{
    public int[] arrayData;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        // �����ﴦ����������
        Debug.Log("Array data length: " + arrayData.Length);
        foreach (int value in arrayData)
        {
            Debug.Log("Array value: " + value);
        }
    }
}
