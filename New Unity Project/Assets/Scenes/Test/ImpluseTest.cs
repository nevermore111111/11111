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
            Debug.Log("��������"); 
            // �ڿո������ʱ������Ч��
            impulseSource.GenerateImpulse();
        }
    }
}
