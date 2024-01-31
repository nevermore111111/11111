using Cinemachine.Utility;
using Lightbug.CharacterControllerPro.Implementation;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Hitted : CharacterState
{
    public bool Check = false;
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
        Debug.Log("进入了受击状态");
        if (CharacterStateController.CurrentState is AttackOnGround)
        {

        }
        if (CharacterActor.isPlayer)
        {

        }
    }
    //根据目标的方位和攻击类型来决定自身的受击类型。需要设置当前的受击动画。
    public void GetHitted(WeaponManager weapon, IAgent.HitKind hitKind, bool NeedChangeState = true)
    {
        Debug.Log("受击了");
        int hitStrength = weapon.weaponOwner.HitStrength;
        HittedBack(weapon, true);
        //击退
        CheckAnimator(weapon, hitKind, NeedChangeState);
        //动画机处理
    }
    public void GetHitted(Vector3 attackDirection,Vector3 hittedForce  ,bool NeedChangeState = true)
    {
        Debug.Log("受击了");


        CharacterActor.RigidbodyComponent.Velocity = Vector3.zero;
        CharacterActor.RigidbodyComponent.AddForce(hittedForce);
    }

    private void CheckAnimator(WeaponManager weapon, IAgent.HitKind hitKind, bool NeedChangeState)
    {
        //这个是转换到世界坐标系。
        Vector3 HitWorldDir = weapon.WeaponWorldDirection;
        Debug.Log($"{weapon.transform.position},{HitWorldDir},{weapon.transform.position + HitWorldDir}");
        if(Check)
        {
            //Debug.DrawLine(weapon.transform.position, weapon.transform.position + HitWorldDir, Color.red,1f);
            //EditorApplication.isPaused = true;
        }

        SetAnimationParameters(weapon.WeaponWorldDirection, true);
        //是否需要切换状态到当前状态
        if (NeedChangeState)
        {
            CharacterStateController.EnqueueTransition<Hitted>();
            CharacterActor.Animator.CrossFadeInFixedTime("Hitted.HittedOnGround", 0.1f, 0,0.1f);
        }
        else
        {
            CharacterActor.Animator.Play("MixHitted", 1, 0.3f);
            CharacterActor.Animator.SetLayerWeight(1, HittedMixWeight);
        }
        PlayerSpecial();
    }

    /// <summary>
    /// 主角特殊方法
    /// </summary>
    private void PlayerSpecial()
    {
        //是否是主角
        if (CharacterActor.isPlayer)
        {

        }
        else
        {

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



    /// <summary>
    /// 根据对方 武器的方向设置自身的受击方向
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="IgnoreYAxis"></param>
    public void SetAnimationParameters(Vector3 WorldAttackDirection, bool IgnoreYAxis = true)
    {
        //世界转换到自身
        Vector3 attackFrom = this.transform.InverseTransformDirection(-WorldAttackDirection);
        if (IgnoreYAxis)
        {
            attackFrom.y = 0f;
        }
        attackFrom.Normalize();
        CharacterActor.Animator.SetFloat("attackXFrom", attackFrom.x);
        CharacterActor.Animator.SetFloat("attackYFrom", attackFrom.z);
        CharacterActor.Animator.SetFloat("attackZFrom", attackFrom.y);
    }
}
