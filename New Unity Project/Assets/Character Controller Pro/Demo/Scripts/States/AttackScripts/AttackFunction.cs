using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// <summary>
//public class AttackFunction : Attack
//{
//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.gameObject.tag == "enemy")
//        {
//            if (OnceAttack == false)
//            {
//                //仅仅只会执行一次卡帧
//                OnceAttack = true;
//                StartCoroutine(ChangeSpeed(0.01f, 0.05f));
//            }
//        }
//    }
//    public IEnumerator ChangeSpeed(float playSpeed, float timeStop)
//    {
//        CharacterActor.Animator.speed = playSpeed;
//        yield return new WaitForSeconds(timeStop);
//        CharacterActor.Animator.speed = 1;
//    }
//}
