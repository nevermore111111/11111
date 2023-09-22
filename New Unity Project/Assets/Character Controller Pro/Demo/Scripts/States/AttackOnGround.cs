using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;

using UnityEngine;
using static Lightbug.CharacterControllerPro.Core.PhysicsActor;

/// <summary>
/// 
/// <summary>
public class AttackOnGround :Attack
{
    public  float gravity = 10;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        
    }
    public override void EnterBehaviour(float dt, CharacterState fromState)
    {
        base.EnterBehaviour(dt, fromState);
        CharacterActor.SetUpRootMotion(true, RootMotionVelocityType.SetVelocity,true,RootMotionRotationType.SetRotation);
        StartCoroutine(CheckAnim());
        ChangeWeaponState(false);
    }

    private IEnumerator CheckAnim()
    {
        yield return null;
        //yield return null;
        Type type = CharacterStateController.PreviousState.GetType();
        bool isPlayMove = CharacterActor.Animator.GetNextAnimatorStateInfo(0).IsTag("AttackOnGround");
        if (CharacterStateController.CurrentState is AttackOnGround && (!isPlayMove))
        {
            Debug.Log("自动切换了");
            if (CharacterActor.IsGrounded && SpAttack == 10)
            {
                CharacterActor.Animator.Play("AttackOnGround.sp01", 0);
                canChangeState = false;
                CharacterActor.ForceNotGrounded();
                //CharacterActor.VerticalVelocity = CharacterActor.Up * 10f;
                Debug.Log("离开地面");
            }
            else if ((type != typeof(Attack)) && type != typeof(StartPlay) && CharacterActor.IsGrounded)
            {
                combo = 1;
                CharacterActor.Animator.SetInteger("combo", Attack.combo);
                CharacterActor.Animator.Play("AttackOnGround.attack01_1", 0);
                canChangeState = false;
            }
            else
            {
                CharacterActor.Animator.Play("GhostSamurai_Common_Idle_Inplace");
            }
        }
    }




    public override void UpdateBehaviour(float dt)
    {
       
        base.UpdateBehaviour(dt);
        if (!canPlayerControl)
        {
            return;
        }
    }



    public override void CheckExitTransition()
    {
        base.CheckExitTransition();
        if (!canPlayerControl)
        {
            return;
        }
        if(canAttackInair&& isNextAttack)
        {
             CharacterStateController.EnqueueTransition<AttackOffGround>();
        }
        else if(CharacterActor.IsGrounded)//这样是在地面
        {
            if (CharacterActions.movement.value != Vector2.zero && canChangeState == true&&SpAttack == -1)
            {
                CharacterStateController.EnqueueTransition<NormalMovement>();
            }
            if (CharacterActor.IsGrounded && isAttack == false && Attack.currentAttackMode == AttackMode.AttackOnGround_fist)
            {
                CharacterStateController.EnqueueTransition<AttackOnGround_fist>();
            }
        }
    }
    public override void ExitBehaviour(float dt, CharacterState toState)
    {
        base.ExitBehaviour(dt, toState);
        ChangeWeaponState(true);
    }


}
