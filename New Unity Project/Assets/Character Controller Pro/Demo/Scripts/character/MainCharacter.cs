using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : CharacterInfo
{
    public CharacterActor CharacterActor;
    public CharacterStateController CharacterStateController;
    public Hitted CharacterHitted;

    public override void GetDamage(float damage, Vector3 pos, WeaponManager weapon, IAgent.HitKind hit = IAgent.HitKind.ground)
    {
        //需要找到主角调用
        CharacterHitted.GetHitted( weapon,hit );
    }


    protected override void Awake()
    {
        base.Awake();
        CharacterActor = GetComponentInParent<CharacterActor>();
        CharacterStateController = this.transform.parent.GetComponentInChildren<CharacterStateController>();
        CharacterHitted = GetComponentInChildren<Hitted>();
    }

#warning(这里没做,摄像机使用的)
    internal bool GetIsAttacked()
    {
        return false;
       
    }

    internal bool GetIsAttacking()
    {
        if(CharacterStateController.CurrentState is Attack)
        {
            return  Attack.isAttack;
        }
        return false;
    }



    internal bool ismoving()
    {
        if (CharacterStateController.CurrentState is NormalMovement)
        {
            NormalMovement normalMovementState = CharacterStateController.CurrentState as NormalMovement;

            // 访问 NormalMovement 实例的变量
            if (normalMovementState != null)
            {

                return normalMovementState.moving;
                // 使用 normalMovementState 访问实例的变量
                // 例如: normalMovementState.SomeVariable
            }
        }
        return false;
    }
}
