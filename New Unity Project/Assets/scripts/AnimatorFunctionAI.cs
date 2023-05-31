using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 
/// <summary>
public class AnimatorFunctionAI : MonoBehaviour
{
    Animator animator;
    private bool isTimeChanged = false;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Stop()
    {

    }
    public void NormalIdle()
    {

    }
    public void AttackEnd()
    {
        animator.applyRootMotion = false;
    }
    public void AttackStart()
    {
        animator.applyRootMotion = true;
    }
    public void MeleeHitFinished()
    {

    }
    public void Attacking() 
    {

    }

    public void SlowDownAnimation(float speed)
    {
        animator.speed = speed;
    }

    public void RestoreAnimationSpeed()
    {
        animator.speed = 1f;
    }

    public void ChangeAnimationTime(float deltaTime)
    {
        if (!isTimeChanged)
        {
            animator.speed = 0.3f;
            isTimeChanged = true;
        }

        StartCoroutine(ResetAnimationTime(deltaTime));
    }





    private System.Collections.IEnumerator ResetAnimationTime(float deltaTime)
    {
        yield return new WaitForSecondsRealtime(deltaTime);
        if (isTimeChanged)
        {
            animator.speed = 1f;
            isTimeChanged = false;
        }
    }

}
