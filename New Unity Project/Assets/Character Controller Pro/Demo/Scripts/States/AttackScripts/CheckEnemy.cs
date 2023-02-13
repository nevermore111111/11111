using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// <summary>
public class CheckEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemy") & (!Attack.enemys.Contains(other.gameObject)))
        {
            Debug.Log("enter" + gameObject);
            Attack.enemys.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("enemy") & Attack.enemys.Contains(other.gameObject))
        {
            Attack.enemys.Remove(other.gameObject);
        }
    }

}
