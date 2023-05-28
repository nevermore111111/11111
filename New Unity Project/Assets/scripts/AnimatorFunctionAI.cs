using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 
/// <summary>
public class AnimatorFunctionAI : MonoBehaviour
{
    Animator Animator;


    public void Start()
    {
        Animator = GetComponent<Animator>();
    }
    public void Stop()
    {

    }
    public void NormalIdle()
    {

    }
    public void AttackEnd()
    {
        Animator.applyRootMotion = false;
    }
    public void AttackStart()
    {
        Animator.applyRootMotion = true;
    }
    public void MeleeHitFinished()
    {

    }
    public void Attacking() 
    {

    }

}
