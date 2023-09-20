using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{

    [SerializeField]
    [Range(0f, 2.0f)] // 使用Range属性来定义可编辑范围
    private float impulseValue = 1.0f; // 初始值

    [SerializeField]
    [Range(0f, 2.0f)]
    private float durationValue = 1.0f; // 初始值



    // 在Inspector面板中显示impulseValue属性
    public float ImpulseForce
    {
        get { return impulseValue; }
        set
        {
            impulseValue = value;
            UpdateLocalScale();
        }
    }

    // 在Inspector面板中显示duration属性
    public float Duration
    {
        get { return durationValue; }
        set
        {
            durationValue = value;
            UpdateLocalScale();
        }
    }

    private void UpdateLocalScale()
    {
        Vector3 newScale = transform.localScale;
        newScale.z = impulseValue;
        newScale.y = durationValue;
        transform.localScale = newScale;
    }
    public Vector3 ImpulseDirection
    {
        get { return transform.forward; }
        set
        {
            // 设置 transform.forward 的新值
            transform.forward = value;
        }
    }
}
