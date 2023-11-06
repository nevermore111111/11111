using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SaveDuringPlay]
public class WeaponData : MonoBehaviour
{
    [Header("�Ƿ��ӡ��������")]
    public bool PrintHit = true;

    public bool isNeedChangeCurrentHit = false;
    public int  ChangeCurrentHitNum = -1;


    [Header("����ű�������ʱ����")]
    [SerializeField]
    [Range(0f,1f)] // ʹ��Range����������ɱ༭��Χ
    public float impulseValue0 = 1.0f; // ��ʼֵ
    [SerializeField]
    [Range(0f, 1f)] // ʹ��Range����������ɱ༭��Χ
    public float impulseValue1 = 1.0f; // ��ʼֵ
    [SerializeField]
    [Range(0f, 1f)] // ʹ��Range����������ɱ༭��Χ
    public float impulseValue2 = 1.0f; // ��ʼֵ
    [SerializeField]
    [Range(0f, 1f)] // ʹ��Range����������ɱ༭��Χ
    public float impulseValue3 = 1.0f; // ��ʼֵ
    [SerializeField]
    [Range(0f, 1f)] // ʹ��Range����������ɱ༭��Χ
    public float impulseValue4 = 1.0f; // ��ʼֵ

    [SerializeField]
    [Range(0f, 1f)]
    public float durationValue0 = 1.0f; // ��ʼֵ
    [SerializeField]
    [Range(0f, 1f)]
    public float durationValue1 = 1.0f; // ��ʼֵ
    [SerializeField]
    [Range(0f, 1f)]
    public float durationValue2 = 1.0f; // ��ʼֵ
    [SerializeField]
    [Range(0f, 1f)]
    public float durationValue3 = 1.0f; // ��ʼֵ
    [SerializeField]
    [Range(0f, 1f)]
    public float durationValue4 = 1.0f; // ��ʼֵ

    [Header("���⹥������")]
    [SerializeField]
    [Range(0f, 1f)]
    public float sp11Duration = 1.0f;
    [SerializeField]
    [Range(0f, 5f)]
    public float sp11Force = 1.0f;

    public CinemachineImpulseDefinition.ImpulseShapes ImpulseShapes = CinemachineImpulseDefinition.ImpulseShapes.Bump;
    public float DropSpeed = 10f;

}
