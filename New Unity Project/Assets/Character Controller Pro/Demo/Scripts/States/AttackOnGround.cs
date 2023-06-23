using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
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
    [Tooltip("角色的武器")]
    public GameObject[] army= new GameObject[1];
    public  float gravity = 10;

    protected override void Awake()
    {
        base.Awake();
        //army[0].SetActive(false);
        //army[1].SetActive(false);
    }
    protected override void Start()
    {
        base.Start();
        
    }
    public override void EnterBehaviour(float dt, CharacterState fromState)
    {
        base.EnterBehaviour(dt, fromState);
        CharacterActor.SetUpRootMotion(true, RootMotionVelocityType.SetPlanarVelocity,true,RootMotionRotationType.AddRotation);
        army[0].SetActive(true);
        army[1].SetActive(true);
        CharacterActor.Animator.CrossFade("GhostSamurai_Common_Idle_Inplace", 0.05f);
    }
    public override void UpdateBehaviour(float dt)
    {
        base.UpdateBehaviour(dt);
        if(!canPlayerControl)
        {
            return;
        }
        UseGravity(dt);
        if (CharacterActions.test.value)
        {
            CharacterActor.Animator.speed = 0.1f;
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

    private void UseGravity(float dt)
    {
        if (!CharacterActor.IsStable)
            CharacterActor.VerticalVelocity += CustomUtilities.Multiply(-CharacterActor.Up, gravity, dt);
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
        if(CharacterActor.IsGrounded && isAttack == false&&attack.currentAttackMode == AttackMode.AttackOnGround_fist)
        {
            CharacterStateController.EnqueueTransition<AttackOnGround_fist>();
        }
    }
    public override void ExitBehaviour(float dt, CharacterState toState)
    {
        base.ExitBehaviour(dt, toState);
        army[0].SetActive(false);
        army[1].SetActive(false);
    }


}
