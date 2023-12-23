using Cinemachine;
using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CinemachineImpulseSource))]
public class Impulse : MonoBehaviour
{
    CinemachineImpulseSource impulseSource;
    static Camera cam;
    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        if (cam == null)
        {
            cam = Camera.main;
        }
    }
    public void GenerateImpulse(Vector3 direction, float strength, float FrequencyGain, float durtion, bool needProject = false)
    {
        if (impulseSource.m_ImpulseDefinition.m_ImpulseType != CinemachineImpulseDefinition.ImpulseTypes.Legacy)
            impulseSource.m_ImpulseDefinition.m_ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Legacy;
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_AttackTime = 0f;
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = durtion * 0.3f;
        impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime = durtion * 0.7f;
        impulseSource.m_ImpulseDefinition.m_AmplitudeGain = FrequencyGain;
        impulseSource.m_ImpulseDefinition.m_AmplitudeGain = strength;
        impulseSource.m_ImpulseDefinition.m_Randomize = false;
        if (needProject)
        {
            direction = ResetDir(direction);
        }
        impulseSource.GenerateImpulse(direction.normalized);
    }

    private Vector3 ResetDir(Vector3 direction)
    {
        return direction.ProjectOntoPlane(cam.transform.forward);
    }
}
