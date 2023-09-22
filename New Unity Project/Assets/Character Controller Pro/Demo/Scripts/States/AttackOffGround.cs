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
        CharacterActor.alwaysNotGrounded = true;
        StartCoroutine(CheckAnim());
        CharacterActor.SetUpRootMotion(true, RootMotionVelocityType.SetVelocity, true, RootMotionRotationType.SetRotation);
        ChangeWeaponState(false);


    }


    private IEnumerator CheckAnim()
    {
        yield return null;
        //yield return null;
        Type type = CharacterStateController.PreviousState.GetType();
        bool isPlayMove = CharacterActor.Animator.GetNextAnimatorStateInfo(0).IsTag("AttackOffGround");
        if (CharacterStateController.CurrentState is AttackOffGround && (!isPlayMove))
        {
            Debug.Log("自动切换了");
            if(CharacterActor.IsGrounded)
            {
                Debug.LogError("为什么在落地状态进入空中攻击？");
            }
            if (!CharacterActor.IsGrounded && SpAttack == 11)
            {
                CharacterActor.Animator.Play("AttackOffGround.sp11", 0);
                canChangeState = false;
            }
            else
            {
                combo = 1;
                CharacterActor.Animator.SetInteger("combo", Attack.combo);
                CharacterActor.Animator.CrossFade("AttackOffGround.air_attack01_1", 0.1f);
                canChangeState = false;
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
        CheckSpAttack();
    }

    private void CheckSpAttack()
    {
        if (CharacterActions.spAttack.value)
        {
            //播放下落的动画
            switch (Attack.currentAttackMode)
            {
                case AttackMode.AttackOnGround:
                    CharacterActor.Animator.SetInteger("specialAttack", 10);
                    //这个执行一个下落攻击
                    break;
                case AttackMode.AttackOnGround_fist:
                    CharacterActor.Animator.SetInteger("specialAttack", 11);
                    break;
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
        if (CharacterActor.IsGrounded)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }

        //if (CharacterActions.movement.value != Vector2.zero && canChangeState == true)
        //{
        //    CharacterStateController.EnqueueTransition<NormalMovement>();
        //}
        //if (CharacterActor.IsGrounded && isAttack == false && Attack.currentAttackMode == AttackMode.AttackOnGround_fist)
        //{
        //    CharacterStateController.EnqueueTransition<AttackOnGround_fist>();
        //}
    }
    public override void ExitBehaviour(float dt, CharacterState toState)
    {
        CharacterActor.alwaysNotGrounded = false;
        base.ExitBehaviour(dt, toState);
        ChangeWeaponState(true);
    }


}
