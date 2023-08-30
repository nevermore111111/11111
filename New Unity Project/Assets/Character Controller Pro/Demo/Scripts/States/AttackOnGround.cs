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
        if ( (type != typeof(Attack))&& type != typeof(StartPlay)&& CharacterActor.IsGrounded)
        {
            combo = 1;
            CharacterActor.Animator.SetInteger("combo", Attack.combo);
            canChangeState = false;
            CharacterActor.Animator.Play("attack01_1");
        }
        else
        {
            CharacterActor.Animator.Play("GhostSamurai_Common_Idle_Inplace");
        }
        CharacterActor.SetUpRootMotion(true, RootMotionVelocityType.SetPlanarVelocity,true,RootMotionRotationType.AddRotation);

        ChangeWeaponState(false);


    }
    public override void UpdateBehaviour(float dt)
    {
        base.UpdateBehaviour(dt);
        if(!canPlayerControl)
        {
            return;
        }
        //在非攻击时
        if (CharacterActions.attack.value)
        {
            //按下攻击键位
            if (canInput)
            {
                canInput = false;
                combo++;
                if (combo > MaxCombo)
                {
                    combo = 1;
                }
                CharacterActor.Animator.SetInteger("combo", combo);
            }
        }
    }



    public override void CheckExitTransition()
    {
        base.CheckExitTransition();
        if (!canPlayerControl)
        {
            return;
        }
        if (!CharacterActor.IsGrounded && isAttack == false)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
        if(CharacterActions.movement.value != Vector2.zero && canChangeState == true)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
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
