using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// <summary>
public class CheckEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject Character;
    [SerializeField]
    private MainCharacter mainCharacter;

    private void Awake()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemy") & (!mainCharacter.enemys.Contains(other.gameObject)))
        {
            Debug.Log("enter" + gameObject);
            mainCharacter.enemys.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("enemy") & mainCharacter.enemys.Contains(other.gameObject))
        {
            mainCharacter.enemys.Remove(other.gameObject);
        }
    }
    private void   Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, Character.transform.position, 10.0f* Time.deltaTime);
    }

}
