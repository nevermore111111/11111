using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{

    [SerializeField]
    [Range(0.1f, 10.0f)] // ʹ��Range����������ɱ༭��Χ
    private float impulseValue = 1.0f; // ��ʼֵ

    [SerializeField]
    [Range(0.1f, 10.0f)]
    private float durationValue = 1.0f; // ��ʼֵ

    // ��Inspector�������ʾimpulse����
    public float Impulse
    {
        get { return impulseValue; }
        set
        {
            impulseValue = value;
            UpdateLocalScale();
        }
    }

    // ��Inspector�������ʾduration����
    public float Duration
    {
        get { return durationValue; }
        set
        {
            durationValue = value;
            UpdateLocalScale();
        }
    }

    // ʹ��setter��getter����������transform.localScale
    private void UpdateLocalScale()
    {
        Vector3 newScale = transform.localScale;
        newScale.z = impulseValue;
        newScale.y = durationValue;
        transform.localScale = newScale;
    }

}
