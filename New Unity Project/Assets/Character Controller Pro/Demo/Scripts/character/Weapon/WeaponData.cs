using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{

    [SerializeField]
    [Range(0.1f, 10.0f)] // 使用Range属性来定义可编辑范围
    private float impulseValue = 1.0f; // 初始值

    [SerializeField]
    [Range(0.1f, 10.0f)]
    private float durationValue = 1.0f; // 初始值

    // 在Inspector面板中显示impulse属性
    public float Impulse
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

    // 使用setter和getter方法来更新transform.localScale
    private void UpdateLocalScale()
    {
        Vector3 newScale = transform.localScale;
        newScale.z = impulseValue;
        newScale.y = durationValue;
        transform.localScale = newScale;
    }

}
