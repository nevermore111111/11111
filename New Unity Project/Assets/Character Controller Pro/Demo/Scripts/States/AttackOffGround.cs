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
        Type type = CharacterStateController.PreviousState.GetType();
        if(CharacterActor.IsGrounded == false)
        {
            if (currentAttackMode == AttackMode.AttackOnGround)
            {
                if (SpAttack == -1)
                {

                }
                else if(SpAttack == 10)
                {

                }
            }
            else if (currentAttackMode == AttackMode.AttackOnGround_fist)
            {
                if(SpAttack == -1)
                {

                }
                else if(SpAttack ==10)
                {

                }
            }
        }
        else
        {
            Debug.LogError("进入空中状态时是地面状态（isground = true）");
        }

       
        CharacterActor.SetUpRootMotion(true, RootMotionVelocityType.SetVelocity, true, RootMotionRotationType.SetRotation);
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
