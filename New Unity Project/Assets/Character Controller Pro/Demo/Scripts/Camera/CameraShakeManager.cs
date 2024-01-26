using UnityEngine;
using DG.Tweening;

public class CameraShakeManager : MonoBehaviour
{
    // Singleton pattern
    private static CameraShakeManager instance;
    public int randomness = 60;
    public static CameraShakeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraShakeManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("CameraShakeManager");
                    instance = obj.AddComponent<CameraShakeManager>();
                }
            }
            return instance;
        }
    }

    // Start camera shake
    public void Shake(float duration = 1f, float strength = 1, int vibrato = 10, float randomness = 90f)
    {
        transform.DOShakePosition(duration, strength, vibrato, randomness, false, true);
    }

    Vector3 shakeTarget = Vector3.zero;
    Vector3 deltaTarget = Vector3.zero;
    //当前震动数量
    int currentShakeNum = 0;
    /// <summary>
    /// 使用dotween
    /// </summary>
    public void Shake(Vector3 shakeDirection, float strength, float frequencyGain, float durtion)
    {
        currentShakeNum++;
        DOTween.Shake(() => shakeTarget, (value) =>
        {
            deltaTarget = value - shakeTarget;
            shakeTarget = value;
        }, durtion, strength * shakeDirection, (int)
        frequencyGain * 10, randomness, true/*是否fadeout*/, ShakeRandomnessMode.Harmonic).OnComplete(() => 
        {
            currentShakeNum--;
            if(currentShakeNum == 0)
            {
                shakeTarget = Vector3.zero;
                deltaTarget = Vector3.zero;
            }
        });
    }
    private void Update()
    {
        if(currentShakeNum != 0)
        {
            transform.position += deltaTarget;
        }
    }
}
