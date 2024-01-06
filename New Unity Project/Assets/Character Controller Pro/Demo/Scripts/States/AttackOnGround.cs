using DG.Tweening;
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
public class AttackOnGround : Attack
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
        CharacterActor.SetUpRootMotion(true, RootMotionVelocityType.SetVelocity, true, RootMotionRotationType.AddRotation);
        StartCoroutine(CheckAnim());
        ChangeWeaponState(false);
    }

    private IEnumerator CheckAnim()
    {
        yield return null;
        //yield return null;
        Type type = CharacterStateController.PreviousState.GetType();
        bool isPlayAttack = CharacterActor.Animator.GetNextAnimatorStateInfo(0).IsTag("AttackOnGround");
        if (CharacterStateController.CurrentState is AttackOnGround && (!isPlayAttack))
        {
            Debug.Log("×Ô¶¯ÇÐ»»ÁË");
            if (canExecute())
            {
                executeStart();
            }
            else if (CharacterActor.IsGrounded && SpAttack == 10)
            {
                CharacterActor.Animator.Play("AttackOnGround.sp01", 0);
                canChangeState = false;
                CharacterActor.ForceNotGrounded();
                //CharacterActor.VerticalVelocity = CharacterActor.Up * 10f;
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
    public override void UpdateIK(int layerIndex)
    {
        if (CharacterActor.CharacterInfo.selectEnemy != null)
        {

        }
    }



    public override void UpdateBehaviour(float dt)
    {
        base.UpdateBehaviour(dt);
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
            if (CharacterActions.movement.value != Vector2.zero && canChangeState == true && SpAttack == -1)
            {
                CharacterStateController.EnqueueTransition<NormalMovement>();
            }
            else if (currentAttackMode == AttackMode.AttackOnGround_fist)
            {
                CharacterStateController.EnqueueTransition<AttackOnGround_fist>();
            }
        }
        else if (!CharacterActor.IsGrounded && !isAttack)//¿ÕÖÐ·Ç¹¥»÷²Å»áÇÐ»»
        {
            if (canAttackInair && isNextAttack)
            {
                CharacterStateController.EnqueueTransition<AttackOffGround>();
            }
            else
            {
                useGravity = true;
                CharacterStateController.EnqueueTransition<NormalMovement>();
            }
        }
    }
    public override void ExitBehaviour(float dt, CharacterState toState)
    {
        base.ExitBehaviour(dt, toState);
        ChangeWeaponState(true);
    }



    //¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¶îÍâ·½·¨·Ö¸îÏß¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
    //¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¶îÍâ·½·¨·Ö¸îÏß¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª
    //¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¶îÍâ·½·¨·Ö¸îÏß¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª¡ª

    /// <summary>
    /// ¿ªÊ¼Ö´ÐÐ´¦¾ö
    /// </summary>
    protected override void executeStart()
    {
        base.executeStart();
        switch (GetExecuteKind())
        {
            case 0:
                CharacterActor.CharacterInfo.selectEnemy.GetDamage(0f, Vector3.one, 0f, "GhostSamurai_Ambushed01_Root");
                CharacterActor.Animator.CrossFadeInFixedTime("Execeute01_back", 0.1f, 0);
                break;
            case 1:
                break;
            default:
                break;
        }

    }
    public void LetSelectEnemyCloser(int requireSkillNum,float TimeDuration)
    {
        SkillReceiver skillReceiver = CharacterActor.CharacterInfo.GetSkillReceiver(requireSkillNum);
        if (CharacterActor.CharacterInfo.selectEnemy != null)
        {
            CharacterActor enemyActor = CharacterActor.CharacterInfo.selectEnemy.characterActor;
            DOTween.To(() => enemyActor.Position, (Value) => { enemyActor.Position = Value; }, skillReceiver.transform.position, TimeDuration);
            
        }
    }
    
    
    private int GetExecuteKind()
    {
        return 0;
    }
}
