using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpluseTest : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("触发了震动"); 
            // 在空格键按下时触发震动效果
            impulseSource.GenerateImpulse();
        }
    }
}
