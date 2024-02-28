using Cinemachine.Utility;
using DG.Tweening;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Lightbug.CharacterControllerPro.Demo.DefenseParameters;

public class Hitted : CharacterState
{
    //进入hitted状态之后，增加一个不可以格挡的时间，如果不特殊说明，就是默认的时间，在这个时间内，不可以做任何的动作，超过这个时间之后，可以进入移动等状态
    public float recoveryTime = 0.3f;//从受击中回复的时间。这个时间内，不可以切换状态
    float curRecoverTime = 0f;
    Tween tweenToChangeRecover;
    public bool CheckDrawToDebug = false;
    [Space(10f)]

    public float HittedForce = 10f;
    public float HittedDrag = 2f;
    public float HittedMixWeight = 0.5f;
    override protected void Start()
    {
        base.Start();
    }
    protected override void Awake()
    {
        base.Awake();
    }
    public override void UpdateBehaviour(float dt)
    {

    }
    public override void EnterBehaviour(float dt, CharacterState fromState)
    {
        HandleRecoverTimeTween();
        if(!CharacterActor.IsPlayer)
        {
            CharacterActor.brain.SetAIBehaviour<AIDefendBehaviour>();
        }
    }



    public override void CheckExitTransition()
    {
        //这个时候需要检查硬直时间
        if (curRecoverTime > 0)
            return;
        else
        {
            if (CharacterActions.defend.value)
                CharacterStateController.EnqueueTransition<NormalMovement>();
        }
    }

    //根据目标的方位和攻击类型来决定自身的受击类型。需要设置当前的受击动画。
    public void GetHitted(WeaponManager weapon, IAgent.HitKind hitKind, bool NeedChangeState = true)
    {
        HittedBack(weapon, true);
        Debug.Log("击退？");
        SetAnimationParameters(weapon.WeaponWorldDirection, true);

        switch (CharacterActor.CharacterInfo.attackAndDefendInfo.currentDenfendKind)
        {
            case DefendKind.unDefend:
                CheckState(weapon, hitKind, NeedChangeState);
                //动画机处理
                return;
            case DefendKind.normalDefend:
                //普通
                return;
            case DefendKind.perfectDefend:
                //完美
                return;
        }
    }

    private void CheckState(WeaponManager weapon, IAgent.HitKind hitKind, bool NeedChangeState)
    {
        Debug.Log("受击了，这时应该修改当前状态到hit");
        CharacterStateController.EnqueueTransition<Hitted>();
        CharacterActor.Animator.CrossFadeInFixedTime("Hitted.HittedOnGround", 0.1f, 0, 0.1f);
    }

    /// <summary>
    /// 击退
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="resetVelocity"></param>
    private void HittedBack(WeaponManager weapon, bool resetVelocity)
    {
        if (resetVelocity)
        {
            CharacterActor.Velocity = Vector3.zero;
        }
        Vector3 targetMove = HittedForce * (weapon.weaponOwner.transform.position - CharacterActor.transform.position).ProjectOntoPlane(Vector3.up).normalized;
        CharacterActor.RigidbodyComponent.AddForce(targetMove);
        CharacterActor.RigidbodyComponent.LinearDrag = HittedDrag;
    }


    public void SetAnimationParameters(Vector3 WorldAttackDirection, DefendKind defendKind)
    {
        switch (defendKind)
        {
            case DefendKind.unDefend:
                SetAnimationParameters(WorldAttackDirection, true, false);
                break;
            default:
                SetAnimationParameters(WorldAttackDirection, false, true);
                break;
        }
    }
    /// <summary>
    /// 根据对方 武器的方向设置自身的受击方向
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="IgnoreYAxis"></param>
    public void SetAnimationParameters(Vector3 WorldAttackDirection, bool IgnoreYAxis = true, bool IgnoreZAxis = false)
    {
        //世界转换到自身
        Vector3 attackFrom = this.transform.InverseTransformDirection(-WorldAttackDirection);
        if (IgnoreYAxis)
        {
            attackFrom.y = 0f;
        }
        if (IgnoreZAxis)
        {
            //完美防御的时候去忽视z轴根据对方的挥砍方向执行设置
            attackFrom.z = 0f;
        }
        attackFrom.Normalize();
        CharacterActor.Animator.SetFloat("attackXFrom", attackFrom.x);
        CharacterActor.Animator.SetFloat("attackYFrom", attackFrom.z);
        CharacterActor.Animator.SetFloat("attackZFrom", attackFrom.y);
    }

    /// <summary>
    /// 返回
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="WeaponDirection"></param>
    /// <returns></returns>
    public bool IsDefend(out float angle, Vector3 WeaponDirection)
    {
        if (CharacterStateController.CurrentState is NormalMovement)
        {
            if (CharacterActor.CharacterInfo.attackAndDefendInfo.currentDenfendKind != DefendKind.unDefend)
            {
                Vector3 ProjectDirection = WeaponDirection.ProjectOntoPlane(CharacterActor.Forward);
                //获得的角度如果是顺时针的，就是正的，否则就是负的。
                angle = Vector3.SignedAngle(Vector3.up, ProjectDirection, CharacterActor.Forward);
                return true;
            }
        }
        angle = 0f;
        return false;
    }


    /// <summary>
    /// 处理硬直时间
    /// </summary>
    private void HandleRecoverTimeTween()
    {
        tweenToChangeRecover?.Kill();
        tweenToChangeRecover = DOTween.To(() => recoveryTime, (value) => curRecoverTime = value, 0f, recoveryTime);
    }
}
