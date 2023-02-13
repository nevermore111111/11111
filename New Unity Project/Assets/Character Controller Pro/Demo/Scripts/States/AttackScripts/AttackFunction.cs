using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// <summary>
public class AttackFunction : Attack
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            Debug.Log("นฅป๗มห");
            StartCoroutine(ChangeSpeed(0.01f,0.1f));
        }
    }
    public IEnumerator ChangeSpeed(float playSpeed, float timeStop)
    {
        CharacterActor.Animator.speed = playSpeed;
        Debug.Log("zhixing");
        yield return new WaitForSeconds(timeStop);
        CharacterActor.Animator.speed = 1;
    }
}
