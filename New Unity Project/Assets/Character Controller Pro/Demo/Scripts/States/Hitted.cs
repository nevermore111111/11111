using Cinemachine.Utility;
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
        Debug.Log("进入了受击状态");
        
    }
    //根据目标的方位和攻击类型来决定自身的受击类型。需要设置当前的受击动画。
    public void GetHitted(WeaponManager weapon, IAgent.HitKind hitKind, bool NeedChangeState = true)
    {
        HittedBack(weapon, true);
        Debug.Log("击退？");
        SetAnimationParameters(weapon.WeaponWorldDirection, true);
        if (CharacterActor.stateController.CurrentState is NormalMovement)
        {
             NormalMovement targetNormalMovement = CharacterActor.stateController.GetState<NormalMovement>() as NormalMovement;
            if(targetNormalMovement != null && targetNormalMovement.IsDefense == true) //正在防御
            {
                if(targetNormalMovement.defenseParameters.currentDenfendKind == DefendKind.perfectDefend) 
                {
                    //完美

                }
                else if(targetNormalMovement.defenseParameters.currentDenfendKind == DefendKind.normalDefend)
                {
                    //普通

                }
                return;
            }
        }
        //击退
        CheckState(weapon, hitKind, NeedChangeState);
        //动画机处理
    }

    private void CheckState(WeaponManager weapon, IAgent.HitKind hitKind, bool NeedChangeState)
    {
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

    /// <summary>
    /// 返回
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="WeaponDirection"></param>
    /// <returns></returns>
    public bool IsDefend(out float angle,Vector3 WeaponDirection)
    {
        if(CharacterStateController.CurrentState is NormalMovement)
        {
            NormalMovement Move = CharacterStateController.CurrentState as  NormalMovement;
            if(Move.defenseParameters.currentDenfendKind!=DefendKind.unDefend)
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

}
