using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
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
    public GameObject[] army= new GameObject[1];
    [Tooltip("进入攻击后，修改角色的高度和宽度，使其不会穿模")]
    public Vector2 HeighAndWidth = new (1f,1.58f) ;
    private Vector2 normalHeightAndWidth;
    public  float gravity = 10;

    protected override void Awake()
    {
        base.Awake();
        army[0].SetActive(false);
        army[1].SetActive(false);
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
        army[0].SetActive(true);
        army[1].SetActive(true);
        CharacterActor.CheckAndSetSize(HeighAndWidth,Lightbug.CharacterControllerPro.Core.CharacterActor.SizeReferenceType.Bottom);

    }
    public override void UpdateBehaviour(float dt)
    {
        base.UpdateBehaviour(dt);
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
                if(combo == 1)
                {
                    Debug.Log("");
                }
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
        if (!CharacterActor.IsGrounded && isAttack == false)
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
        army[0].SetActive(false);
        army[1].SetActive(false);
        CharacterActor.CheckAndSetSize(normalHeightAndWidth, Lightbug.CharacterControllerPro.Core.CharacterActor.SizeReferenceType.Bottom);
    }


}
