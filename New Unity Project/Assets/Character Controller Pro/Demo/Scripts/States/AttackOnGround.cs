using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using UnityEditor.Animations;
using UnityEditorInternal;
using UnityEngine;
using static Lightbug.CharacterControllerPro.Core.PhysicsActor;

/// <summary>
/// 
/// <summary>
public class AttackOnGround :Attack
{
    [Tooltip("角色的武器")]
    public GameObject army;
    [Tooltip("进入攻击后，修改角色的高度和宽度，使其不会穿模")]
    public Vector2 HeighAndWidth = new (1f,1.58f) ;
    private Vector2 normalHeightAndWidth;

    protected override void Awake()
    {
        base.Awake();
        army.SetActive(false);
    }
    protected override void Start()
    {
        base.Start();
        normalHeightAndWidth = CharacterActor.BodySize;
    }
    public override void EnterBehaviour(float dt, CharacterState fromState)
    {
        base.EnterBehaviour(dt, fromState);
        CharacterActor.SetUpRootMotion(true, RootMotionVelocityType.SetPlanarVelocity,true,RootMotionRotationType.AddRotation);
        army.SetActive(true);
        CharacterActor.CheckAndSetSize(HeighAndWidth,Lightbug.CharacterControllerPro.Core.CharacterActor.SizeReferenceType.Bottom);
    }
    public override void UpdateBehaviour(float dt)
    {
        base.UpdateBehaviour(dt);
        if(CharacterActions.test.value)
        {
            CharacterActor.Animator.speed = 0.1f;
        }
        //在非攻击时
        if (CharacterActions.attack.value)
        {
            //按下攻击键位
            if(canInput)
            {
                canInput = false;
                combo++;
                if(combo > MaxCombo)
                {
                    combo = 1;
                }
                CharacterActor. Animator.SetInteger("combo", combo);
            }
        }
    }
    public override void CheckExitTransition()
    {
        base.CheckExitTransition();
        if (!CharacterActor.IsGrounded)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
        if(CharacterActions.movement.value != Vector2.zero && canChangeState == true)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }

    }
    public override void ExitBehaviour(float dt, CharacterState toState)
    {
        base.ExitBehaviour(dt, toState);
        army.SetActive(false);
        CharacterActor.CheckAndSetSize(normalHeightAndWidth, Lightbug.CharacterControllerPro.Core.CharacterActor.SizeReferenceType.Bottom);
    }


}
