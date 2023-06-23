using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyCartPlus : MonoBehaviour
{
    CinemachineDollyCart CinemachineDollyCart;
    public AnimationCurve curve;
    public float ChangeSpeed;
    private float originalSpeed;
    private float targetSpeed;
    private float startTime;
    private float duration;

    public void Start()
    {
        CinemachineDollyCart = GetComponent<CinemachineDollyCart>();
        originalSpeed = CinemachineDollyCart.m_Speed;
    }

    public void SetSpeedByCurve(float newSpeed)
    {
        startTime = Time.time;
        duration = ChangeSpeed;
        targetSpeed = newSpeed;
        StartCoroutine(ChangeSpeedCoroutine());
    }

    private IEnumerator ChangeSpeedCoroutine()
    {
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            float curveValue = curve.Evaluate(t);
            float currentSpeed = originalSpeed + curveValue * (targetSpeed - originalSpeed);
            CinemachineDollyCart.m_Speed = currentSpeed;
            yield return null;
        }

        // Ensure the final speed is set correctly after the duration
        CinemachineDollyCart.m_Speed = targetSpeed;
    }
}
