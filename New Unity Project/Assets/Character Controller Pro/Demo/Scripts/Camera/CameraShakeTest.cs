using DG.Tweening;
using UnityEngine;
using Cinemachine;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;

public class CameraShakeTest : MonoBehaviour
{
    Vector3 shake = Vector3.zero;
    CinemachineBrain brain;
    CinemachineVirtualCameraBase currentCamera;

    public void Awake()
    {
        TryGetComponent(out brain);
    }

    private float getCurCameraDistance()
    {
        if (brain != null && brain.ActiveVirtualCamera != null)
        {
            ICinemachineCamera icamera = brain.ActiveVirtualCamera;
            return (icamera.LookAt.position - transform.position).magnitude;
        }
        Debug.LogError("!错误");
        return -1f;
    }

    public void Shake(Transform targetTransform, float strength, float stime, int vibrate)
    {
        strength = strength * getCurCameraDistance();
        shake = Vector3.zero;
        DOTween.Shake(() => shake, v => shake = v, stime, strength, vibrate, 90f, ignoreZAxis: false, true, 0)
             .SetTarget(shake)
             .SetSpecialStartupMode(SpecialStartupMode.SetShake)
             .SetOptions(false)
             .OnUpdate(() =>
             {
                 // 在每一帧更新传入的Transform的位置
                 targetTransform.position += shake;
             })
             .OnComplete(() =>
             {
                 shake = Vector3.zero;
             });
    }

    public void ShakeCamera2(float strength, float stime, int vibrate)
    {
        strength = strength * getCurCameraDistance();

        DOTween.Shake(() => Vector3.zero, v =>
        {
            // 在每一帧更新摄像机位置
            transform.position += v;
        }, stime, strength, vibrate, 90f, ignoreZAxis: false, true, 0)
        .SetSpecialStartupMode(SpecialStartupMode.SetShake)
        .SetOptions(false);
    }
}
