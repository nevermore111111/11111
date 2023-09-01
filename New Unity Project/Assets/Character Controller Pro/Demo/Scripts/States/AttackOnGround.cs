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
        Type type =CharacterStateController.PreviousState.GetType();
        if(CharacterActor.IsGrounded&&Attack.spAttack ==10)
        {
            CharacterActor.Animator.Play("AttackOnGround.sp01", 0);
            canChangeState = false;
            CharacterActor.ForceNotGrounded();
            CharacterActor.VerticalVelocity = CharacterActor.Up * 10f;
            Debug.Log("离开地面");
        }
        else if( (type != typeof(Attack))&& type != typeof(StartPlay)&& CharacterActor.IsGrounded)
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
       CharacterActor.SetUpRootMotion(true, RootMotionVelocityType.SetVelocity,true,RootMotionRotationType.AddRotation);

        ChangeWeaponState(false);

    }
    public override void UpdateBehaviour(float dt)
    {
        if (spAttack == 10)
            CharacterActor.ForceNotGrounded();
        base.UpdateBehaviour(dt);
        if(!canPlayerControl)
        {
            return;
        }
        //在非攻击时
      
    }



    public override void CheckExitTransition()
    {
        base.CheckExitTransition();
        if (!canPlayerControl)
        {
            return;
        }
        if (!CharacterActor.IsGrounded && isAttack == false&&Attack.spAttack ==-1)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
        if(CharacterActions.movement.value != Vector2.zero && canChangeState == true)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
        if(CharacterActor.IsGrounded && isAttack == false && CharacterActions.spAttack.value == true)
        {
            CharacterStateController.EnqueueTransition<AttackOffGround>();
        }
        if(CharacterActor.IsGrounded && isAttack == false&& Attack.currentAttackMode == AttackMode.AttackOnGround_fist)
        {
            CharacterStateController.EnqueueTransition<AttackOnGround_fist>();
        }
    }
    public override void ExitBehaviour(float dt, CharacterState toState)
    {
        base.ExitBehaviour(dt, toState);
        ChangeWeaponState(true);
    }


}
