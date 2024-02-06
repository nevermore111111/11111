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
        CharacterActor.Animator.SetTrigger(AttackOnGroundPar);
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
            Debug.Log("自动切换了");
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
                CharacterActor.Animator.SetInteger("combo", attack.combo);
                CharacterActor.Animator.Play("AttackOnGround.attack01_1", 0);
                canChangeState = false;
            }
            else
            {
                CharacterActor.Animator.Play("GhostSamurai_Common_Idle_Inplace");
            }
        }
    }
    //public override void UpdateIK(int layerIndex)
    //{
    //    if (CharacterActor.CharacterInfo.selectEnemy != null)
    //    {
    //        foreach (var iKParChild in IKPar)
    //        {
    //            if (iKParChild.targetTransform && (iKParChild.ikRotateWeight != 0 || iKParChild.ikPosWeight != 0))
    //            {
    //                SetIKbyIKPar(iKParChild);
    //                Debug.Log("Ik");
    //            }
    //        }
    //    }
    //}



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
        else if (!CharacterActor.IsGrounded && !isAttack)//空中非攻击才会切换
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
        CharacterActor.Animator.ResetTrigger(AttackOnGroundPar);
        ChangeWeaponState(true);
    }



    //——————————————————————————————————————————————————————————————————额外方法分割线——————————————————————————————————————————————————————————————————
    //——————————————————————————————————————————————————————————————————额外方法分割线——————————————————————————————————————————————————————————————————
    //——————————————————————————————————————————————————————————————————额外方法分割线——————————————————————————————————————————————————————————————————

    /// <summary>
    /// 开始执行处决
    /// </summary>
    protected override void executeStart()
    {
        base.executeStart();
        switch (GetExecuteKind())
        {
            case 0:
                
                LetSelectEnemyCloser(0, 0.1f);
                break;
            case 1:
                break;
            default:
                break;
        }

    }
    //关闭所有的碰撞盒，并且把他拉过来，并且开启ik
    public void LetSelectEnemyCloser(int requireSkillNum, float TimeDuration)
    {
        SkillReceiver skillReceiver = CharacterActor.CharacterInfo.GetSkillReceiver(requireSkillNum);
        if (CharacterActor.CharacterInfo.selectEnemy != null)
        {
            CharacterActor.UseRootMotion = true;

            //最小化敌人刚体，关闭敌人碰撞，开启ik
            CharacterActor enemyActor = CharacterActor.CharacterInfo.selectEnemy.characterActor;
            CharacterActor.CheckAndSetSize(new Vector2(0.2f, 1.58f));
            CharacterActor.UseRootMotion = true;
            CharacterActor.Velocity = Vector3.zero;
            enemyActor.UseRootMotion = true;
            //IKPar[0].targetTransform = CharacterActor.CharacterInfo.selectEnemy.allSkillReceivers.FirstOrDefault(_ => _.skillPoint == 1001)?.transform;
            StopRigidBody(enemyActor);
            CharacterActor.CharacterInfo.selectEnemy.GetDamage(0f, Vector3.one, 0f, "GhostSamurai_Ambushed01_Root");
            CharacterActor.Animator.CrossFadeInFixedTime("Execeute01_back", 0.1f, 0);
            DOTween.To(() => enemyActor.Position, (Value) =>
            {
                enemyActor.Position = Value;
                enemyActor.Velocity = Vector3.zero;
                CharacterActor.Velocity = Vector3.zero;
                //Debug.Log((enemyActor.Position - skillReceiver.transform.position).magnitude);
            }, skillReceiver.transform.position, TimeDuration).OnComplete(() =>
            {
                enemyActor.Position = skillReceiver.transform.position;
                CharacterActor.Velocity = Vector3.zero;
            }
            );
            DOTween.To(() => enemyActor.Forward,(value) => { enemyActor.Forward = value; },skillReceiver.transform.forward, TimeDuration).
                OnComplete(() => 
            {
                enemyActor.Forward = skillReceiver.transform.forward;
            });
        }
    }

    private void TestFun0001(CharacterActor enemyActor)
    {
        Debug.Log("测试方法");
        CharacterActor.Animator.speed = 0.1f;
        enemyActor.Animator.speed = 0.1f;
    }

    //最小化敌人刚体，关闭敌人碰撞
    private static void StopRigidBody(CharacterActor enemyActor)
    {
        enemyActor.SetSize(new Vector2(0.01f, 1.7f), CharacterActor.SizeReferenceType.Bottom);


        foreach (var receiver in enemyActor.CharacterInfo.allReceives)
        {
            receiver.gameObject.SetActive(false);
        }
    }

    private int GetExecuteKind()
    {
        return 0;
    }
}
