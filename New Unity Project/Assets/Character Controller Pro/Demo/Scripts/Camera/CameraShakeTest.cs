using DG.Tweening.Core.Enums;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening.Core;

public class CameraShakeTest : MonoBehaviour
{
    Vector3 shake = Vector3.zero;
    CinemachineBrain brain;
    CinemachineVirtualCameraBase currentCamera;

    public void Awake()
    {
        TryGetComponent(out brain);
    }

    public void ShakeCamera2(float strength, float stime, int vibrate)
    {
        strength = strength * getCurCameraDistance();
        shake = Vector3.zero;
        DOTween.Shake(() => shake, delegate (Vector3 x)
        {
            shake = x;
        }, stime, strength, vibrate, 90f, ignoreZAxis: false, true, 0).SetTarget(shake).SetSpecialStartupMode(SpecialStartupMode.SetShake)
            .SetOptions(false).OnComplete(() =>
            {
                // shake = Vector3.zero;
            });
    }
    private float getCurCameraDistance()
    {
        if (brain != null && brain.ActiveVirtualCamera != null)
        {
            ICinemachineCamera icamera = brain.ActiveVirtualCamera;
            return (icamera.LookAt.position - transform.position).magnitude;
        }
        Debug.LogError("!´íÎó");
        return -1f;
    }
}
