using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
    public float impulse
    {
        get { return transform.localScale.z; }
        set
        {
            // 设置transform.localScale.x的新值
            Vector3 newScale = transform.localScale;
            newScale.z = value;
            transform.localScale = newScale;
        }
    }
    public float duration
    {
        get { return transform.localScale.y; }
        set
        {
            Vector3 newScale = transform.localScale;
            newScale.y = value;
            transform.localScale = newScale;
        }
    }

}
