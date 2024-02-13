using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class CameraShakeManager : MonoBehaviour
{
    // Singleton pattern
    private static CameraShakeManager instance;
    public int randomness = 60;
    public float StrengthPara = 0.2f;
    public int frequencyGainPara = 20;
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
    //public void Shake(float duration = 1f, float strength = 1, int vibrato = 10, float randomness = 90f)
    //{
    //    transform.DOShakePosition(duration, strength, vibrato, randomness, false, true);
    //}

    Vector3 shakeTarget = Vector3.zero;
    Vector3 deltaTarget = Vector3.zero;
    //当前震动数量
    int currentShakeNum = 0;
    /// <summary>
    /// 使用dotween
    /// </summary>
    public void Shake(Vector3 shakeDirection, float strength, float frequencyGain, float durtion)
    {
        Debug.Log("震动了");
        Debug.Log($"震动强度{strength}，震动频率{frequencyGain}，震动时间{durtion}，注意这里频率和强度我都加了系数，自己去代码里看");
        currentShakeNum++;
        DOTween.Shake(() => shakeTarget, (value) =>
        {
            deltaTarget = value - shakeTarget;
            shakeTarget = value;
        }, durtion, strength * shakeDirection* StrengthPara, (int)
        (frequencyGain * frequencyGainPara), randomness, true/*是否fadeout*/, ShakeRandomnessMode.Full).OnComplete(() => 
        {
            currentShakeNum--;
            if(currentShakeNum == 0)
            {
                shakeTarget = Vector3.zero;
                deltaTarget = Vector3.zero;
            }
        });
        //transform.DOShakePosition()
    }
    private void LateUpdate()
    {
        if(currentShakeNum != 0)
        {
            transform.position += deltaTarget;
        }
    }
    [Range(0,5)]
    public float TestStrength = 2f;
    [Range(0,2)]
    public float TestFrequence = 1.0f;
    [Range(0,1)]
    public float TestDuration = 0.4f;

    [Button("震动测试")]
    public void TestFun0001() 
    {
        Debug.Log("震动测试");
        Shake(Vector3.up, TestStrength, TestFrequence, TestDuration);
    }
}
