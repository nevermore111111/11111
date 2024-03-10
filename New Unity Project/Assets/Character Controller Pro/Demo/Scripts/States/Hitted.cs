using Cinemachine.Utility;
using DG.Tweening;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using OfficeOpenXml.Drawing.Style.Fill;
using Rusk;
using UnityEngine;

public class Hitted : CharacterState
{
    //进入hitted状态之后，增加一个不可以格挡的时间，如果不特殊说明，就是默认的时间，在这个时间内，不可以做任何的动作，超过这个时间之后，可以进入移动等状态
    public float recoveryTime = 0.3f;//从受击中回复的时间。这个时间内，不可以切换状态
    public float AutoChangeTime = 1.1f;
    float curRecoverTime = 0f;
    float curAutoChangeTime = 0f;
    Tween tweenToChangeRecover;
    Tween TweenToChangeAuto;
    public bool CheckDrawToDebug = false;
    [Space(10f)]

    public float HittedForce = 10f;
    public float HittedDrag = 2f;
    public float HittedMixWeight = 0.5f;
    //public bool AlwaysPerfectDefend = false;

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
        HandleAutoRecoverTimeTween();
        HandleRecoverTimeTween();
        if (!CharacterActor.IsPlayer)
        {
            CharacterActor.brain.SetAIBehaviour<AIDefendBehaviour>();
        }
    }

    private void HandleAutoRecoverTimeTween()
    {
        TweenToChangeAuto?.Kill();
        TweenToChangeAuto = DOTween.To(() => AutoChangeTime, (_) => { curAutoChangeTime = _; }, 0, AutoChangeTime);
    }


    public override void CheckExitTransition()
    {
        //这个时候需要检查硬直时间
        if (curRecoverTime > 0)
            return;
        else if (curAutoChangeTime > 0)//还不会自动切换
        {
            if (CharacterActions.defend.value || CharacterActions.jump.value)
                CharacterStateController.EnqueueTransition<NormalMovement>();
            if (CharacterActions.evade.value)
                CharacterStateController.EnqueueTransition<Evade>();
        }
        else
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
    }

    //根据目标的方位和攻击类型来决定自身的受击类型。需要设置当前的受击动画。
    public void GetHitted(WeaponManager weapon, IAgent.HitKind hitKind)
    {
        if (CharacterActor.IsPlayer)
            HittedBack(weapon, true);
        Debug.Log("击退？");

        SetAnimationParameters(weapon.WeaponWorldDirection, DefendKind.perfectDefend);
        CheckAndEnterState(weapon, hitKind, DefendKind.perfectDefend);

    }

    private void CheckAndEnterState(WeaponManager weapon, IAgent.HitKind hitKind, DefendKind defend)
    {
        switch (defend)
        {
            case DefendKind.unDefend:
                CharacterStateController.EnqueueTransition<Hitted>();
                CharacterActor.Animator.CrossFadeInFixedTime("Hitted.HittedOnGround", 0.1f, 0, 0.1f);

                //未防御
                break;
            case DefendKind.normalDefend:
                CharacterActor.Animator.CrossFadeInFixedTime("NormalMovement.StableGrounded", 0.05f, 0, 0f);
                weapon.PlaySound("normal");
                //普通
                break;
            case DefendKind.perfectDefend:
                CharacterActor.Animator.CrossFadeInFixedTime("NormalMovement.PerfectDefend", 0.1f, 0, 0.08f);
                CharacterActor.Animator.ResetTrigger(defendOnce);
                weapon.PlaySound("perfect");
                //完美
                break;
            case DefendKind.OnlyDamage:
                Debug.LogError("这个地方需要覆写动画");//这个是小受击
                //霸体
                break;
            case DefendKind.noDamage:
                Debug.Log("现在处于无敌");
                //无敌
                break;
        }
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


    //public void SetAnimationParameters(Vector3 WorldAttackDirection, DefendKind defendKind)
    //{
    //    switch (defendKind)
    //    {
    //        case DefendKind.unDefend:
    //            SetAnimationParameters(WorldAttackDirection, true, false);
    //            break;
    //        default:
    //            SetAnimationParameters(WorldAttackDirection, false, true);
    //            break;
    //    }
    //}
    /// <summary>
    /// 根据对方 武器的方向设置自身的受击方向
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="IgnoreYAxis"></param>
    public void SetAnimationParameters(Vector3 WorldAttackDirection, DefendKind CurrentDefendKind)
    {
        CharacterActor.Animator.SetFloat(perfectDefendKind, 0);
        CharacterActor.Animator.SetTrigger(defendOnce);

        //防御住的忽略z，未防御住的忽略y
        bool IgnoreYAxis = false;
        bool IgnoreZAxis = false;
        switch (CurrentDefendKind)
        {
            case DefendKind.perfectDefend:
                IgnoreZAxis = true;
                break;
            case DefendKind.normalDefend:
                IgnoreZAxis = true;
                break;
            case DefendKind.unDefend:
                IgnoreYAxis = true;
                break;
            case DefendKind.OnlyDamage://无所谓
                break;
            case DefendKind.noDamage://无所谓
                break;
        }
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
        if (CharacterActor.IsPlayer && CharacterActor.CharacterInfo.attackAndDefendInfo.currentDenfendKind == DefendKind.unDefend)
        {
            attackFrom *= 0.55f;//修正动画
        }
        CharacterActor.Animator.SetFloat("attackXFrom", attackFrom.x);
        CharacterActor.Animator.SetFloat("attackYFrom", attackFrom.z);
        CharacterActor.Animator.SetFloat("attackZFrom", attackFrom.y);
        if (CurrentDefendKind == DefendKind.perfectDefend && CharacterActor.IsPlayer)
        {
            Debugger.Log("进入了完美格挡");
            Vector2 defendVector = new Vector2(attackFrom.x, attackFrom.y);
            if (defendVector.x > 0f)//根据攻击方向象限防御
            {
                if (defendVector.y > 0f)
                {
                    CharacterActor.Animator.SetFloat(perfectDefendKind, 0.85f);//不是整数是因为动画不适合
                }
                else
                {
                    CharacterActor.Animator.SetFloat(perfectDefendKind, 4f);
                }
            }
            else
            {
                if (defendVector.y > 0f)
                {
                    CharacterActor.Animator.SetFloat(perfectDefendKind, 2.25f);
                }
                else
                {
                    CharacterActor.Animator.SetFloat(perfectDefendKind, 3f);
                }
            }
        }
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
