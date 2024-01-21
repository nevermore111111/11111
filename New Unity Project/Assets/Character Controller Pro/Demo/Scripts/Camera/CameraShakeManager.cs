using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    [Range(0, 1f)]
    public float duration;
    [Range(0, 1f)]
    public float strength;
    public Vector3 strengthDir = Vector3.up;
    [Tooltip("归一化的方向")]
    public Vector3 StrengthDir
    {
        get { return strengthDir; }
        set { strengthDir = value.normalized; }
    }
    public int vibrato = 10;
    public float randomness = 50;
    public ShakeRandomnessMode shakeMode = ShakeRandomnessMode.Full;
    public Ease targetEase = Ease.OutQuart;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("调用shake");
            Shake();
        }
    }

    Vector3 targetShake = Vector3.zero;
    Vector3 delVector = Vector3.zero;
    int shakeNum = 0;
    public Tween Shake()
    {
        return DOTween.Shake(() => targetShake, (_) =>
        {
            delVector = _ - targetShake;
            targetShake = _;
        }, duration, strength * strengthDir, vibrato, randomness, true, shakeMode).OnStart(() => shakeNum += 1).OnComplete(() => shakeNum -= 1).SetEase(targetEase);

    }
    public void LateUpdate()
    {
        if (shakeNum != 0)
        {
            transform.localPosition += delVector;
        }
    }
}
