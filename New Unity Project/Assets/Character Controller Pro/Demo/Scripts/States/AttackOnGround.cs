using DG.Tweening;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            Debug.Log("徭強俳算阻");
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
            foreach (var iKParChild in IKPar)
            {
                if (iKParChild.targetTransform && (iKParChild.ikRotateWeight != 0 || iKParChild.ikPosWeight != 0))
                    SetIKbyIKPar(iKParChild);
            }
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
        else if (!CharacterActor.IsGrounded && !isAttack)//腎嶄掲好似嘉氏俳算
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



    //！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！駆翌圭隈蛍護�漾�！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
    //！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！駆翌圭隈蛍護�漾�！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
    //！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！駆翌圭隈蛍護�漾�！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！

    /// <summary>
    /// 蝕兵峇佩侃畳
    /// </summary>
    protected override void executeStart()
    {
        base.executeStart();
        switch (GetExecuteKind())
        {
            case 0:
                CharacterActor.CharacterInfo.selectEnemy.GetDamage(0f, Vector3.one, 0f, "GhostSamurai_Ambushed01_Root");
                CharacterActor.Animator.CrossFadeInFixedTime("Execeute01_back", 0.1f, 0);
                LetSelectEnemyCloser(0, 0.1f);
                break;
            case 1:
                break;
            default:
                break;
        }

    }
    //購液侭嗤議当弉歳��旺拝委麿性狛栖��旺拝蝕尼ik
    public void LetSelectEnemyCloser(int requireSkillNum, float TimeDuration)
    {
        SkillReceiver skillReceiver = CharacterActor.CharacterInfo.GetSkillReceiver(requireSkillNum);
        if (CharacterActor.CharacterInfo.selectEnemy != null)
        {
            CharacterActor enemyActor = CharacterActor.CharacterInfo.selectEnemy.characterActor;
            Debug.Log(CharacterActor.CharacterInfo.selectEnemy.allSkillReceivers.FirstOrDefault(_ => _.skillPoint == 1001));
            IKPar[0].targetTransform = CharacterActor.CharacterInfo.selectEnemy.allSkillReceivers.FirstOrDefault(_ => _.skillPoint == 1001)?.transform;
            foreach (var receiver in CharacterActor.CharacterInfo.allReceives)
            {
                receiver.enabled = false;
            }
            DOTween.To(() => enemyActor.Position, (Value) => { enemyActor.Position = Value; }, skillReceiver.transform.position, TimeDuration);
        }
    }


    private int GetExecuteKind()
    {
        return 0;
    }
}
