using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using System;

using UnityEngine;
using static Lightbug.CharacterControllerPro.Core.PhysicsActor;

/// <summary>
/// 
/// <summary>
public class AttackOnGround_fist : Attack
{
    [Tooltip("角色的武器")]
    public GameObject[] army = new GameObject[1];
   
    public float gravity = 10;
    //这个是使用sp02技能后起跳的速度。
    public float upVerticalVelocity = 10f;

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
        CharacterActor.SetUpRootMotion(true, RootMotionVelocityType.SetVelocity, true, RootMotionRotationType.AddRotation);
        ChangeWeaponState(false);
        Type type = CharacterStateController.PreviousState.GetType();
        if (CharacterActor.IsGrounded && SpAttack == 11)
        {
            CharacterActor.Animator.Play("AttackOnGround_fist.sp02", 0);
            SpAttack = -1;
            canChangeState = false;
            CharacterActor.ForceNotGrounded();
            CharacterActor.VerticalVelocity = new Vector3(0, upVerticalVelocity, 0);
        }
        if (CharacterActor.IsGrounded)
        {
            combo = 1;
            CharacterActor.Animator.Play("AttackOnGround_fist.attack01_1", 0);
            CharacterActor.Animator.SetInteger("combo", attack.combo);
            canChangeState = false;
        }
        else
        {
            CharacterActor.Animator.Play("AttackOnGround_fist.Lucy_Idle");
        }
        //army[0].SetActive(true);
        //army[1].SetActive(true);


    }
    public override void UpdateBehaviour(float dt)
    {
        base.UpdateBehaviour(dt);
        //在非攻击时
    }
    public override void CheckExitTransition()
    {
        base.CheckExitTransition();
        if (!CharacterActor.IsGrounded && isAttack == false)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
        if (CharacterActions.movement.value != Vector2.zero && canChangeState == true)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
        if (CharacterActor.IsGrounded && isAttack == false && currentAttackMode == AttackMode.AttackOnGround)
        {
            CharacterStateController.EnqueueTransition<AttackOnGround>();
        }
    }
    public override void ExitBehaviour(float dt, CharacterState toState)
    {
        base.ExitBehaviour(dt, toState);
        ChangeWeaponState(true);
        //army[0].SetActive(false);
        //army[1].SetActive(false);
    }


}
