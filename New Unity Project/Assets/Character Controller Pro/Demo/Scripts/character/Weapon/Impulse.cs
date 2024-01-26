using Cinemachine;
using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[RequireComponent(typeof(CinemachineImpulseSource))]
public class Impulse : MonoBehaviour
{
    CinemachineImpulseSource impulseSource;
    static Camera cam;
    //新增一个使用dotween来设置震动的方法，传入的参数相同但是使用dotween这样方便操作
    enum SkakeAssetKind
    {
        normal,
        special
    }
    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        if (cam == null)
        {
            cam = Camera.main;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="strength"></param>
    /// <param name="FrequencyGain"></param>
    /// <param name="durtion"></param>
    /// <param name="needProject"></param>
    public void GenerateImpulse(Vector3 direction, float strength, float FrequencyGain, float durtion)
    {
        if (impulseSource.m_ImpulseDefinition.m_ImpulseType != CinemachineImpulseDefinition.ImpulseTypes.Legacy)
            impulseSource.m_ImpulseDefinition.m_ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Legacy;
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = 0f;
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = durtion * 0.3f;
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = durtion * 0.7f;
        impulseSource.m_ImpulseDefinition.m_AmplitudeGain = FrequencyGain;
        impulseSource.m_ImpulseDefinition.m_AmplitudeGain = strength;
        impulseSource.m_ImpulseDefinition.m_Randomize = false;
        //if (needProject)
        //{
        //    direction = ResetDir(direction);
        //}
        impulseSource.GenerateImpulse(direction.normalized);
    }



   
    private void SetShakeKind(SkakeAssetKind shakeAssetKind)
    {
        switch (shakeAssetKind)
        {
            case (SkakeAssetKind.normal):
                {
                    break;
                }
            case (SkakeAssetKind.special):
                {
                    break;
                }
        }
    }

    private Vector3 ResetDir(Vector3 direction)
    {
        return direction.ProjectOntoPlane(cam.transform.forward);
    }
}
