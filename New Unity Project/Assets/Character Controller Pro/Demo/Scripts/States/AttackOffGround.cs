using Lightbug.CharacterControllerPro.Core;
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
public class AttackOffGround : Attack
{
    public float gravity = 10;
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
        Type type = CharacterStateController.PreviousState.GetType();
        if (type != typeof(StartPlay) && type == typeof(AttackOnGround))//这个是我从什么地方进入这个状态，然后进入时播放不同的动画
        {
            combo = 1;
            CharacterActor.Animator.SetInteger("combo", Attack.combo);
            canChangeState = false;
            CharacterActor.Animator.Play("attack01_1");
        }
        else if(type == typeof(AttackOnGround_fist))
        {
            combo = 1;
            CharacterActor.Animator.SetInteger("combo", Attack.combo);
            canChangeState = false;
            CharacterActor.Animator.Play("attack01_1");
        }
        else if(!CharacterActor.IsGrounded)
        {
            //这是在空中某个状态进入
        }
        CharacterActor.SetUpRootMotion(true, RootMotionVelocityType.SetPlanarVelocity, true, RootMotionRotationType.SetRotation);

        ChangeWeaponState(false);


    }
    public override void UpdateBehaviour(float dt)
    {
        base.UpdateBehaviour(dt);
        if (!canPlayerControl)
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
        if (CharacterActions.movement.value != Vector2.zero && canChangeState == true)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
        if (CharacterActor.IsGrounded && isAttack == false && Attack.currentAttackMode == AttackMode.AttackOnGround_fist)
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
