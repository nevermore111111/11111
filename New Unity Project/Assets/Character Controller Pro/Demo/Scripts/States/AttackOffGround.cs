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
            Debug.LogError("�������״̬ʱ�ǵ���״̬��isground = true��");
        }

        //if (type != typeof(StartPlay) && type == typeof(AttackOnGround))//������Ҵ�ʲô�ط��������״̬��Ȼ�����ʱ���Ų�ͬ�Ķ���
        //{
        //    combo = 1;
        //    CharacterActor.Animator.SetInteger("combo", Attack.combo);
        //    canChangeState = false;
        //    CharacterActor.Animator.Play("attack01_1");
        //}
        //else if (type == typeof(AttackOnGround_fist))
        //{
        //    combo = 1;
        //    CharacterActor.Animator.SetInteger("combo", Attack.combo);
        //    canChangeState = false;
        //    CharacterActor.Animator.Play("attack01_1");
        //}
        //else if (!CharacterActor.IsGrounded)
        //{
        //    //�����ڿ���ĳ��״̬����
        //}
        CharacterActor.SetUpRootMotion(true, RootMotionVelocityType.SetVelocity, true, RootMotionRotationType.SetRotation);
        ChangeWeaponState(false);


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
            //��������Ķ���
            switch (Attack.currentAttackMode)
            {
                case AttackMode.AttackOnGround:
                    CharacterActor.Animator.SetInteger("specialAttack", 10);
                    //���ִ��һ�����乥��
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
