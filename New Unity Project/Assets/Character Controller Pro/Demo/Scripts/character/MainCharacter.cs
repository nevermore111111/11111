using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : CharacterInfo ,IAgent
{
    public CharacterActor CharacterActor;
    public CharacterStateController CharacterStateController;
    public

     void GetDamage(float damage, Vector3 pos)
    {
        
    }


    protected override void Awake()
    {
        base.Awake();
        CharacterActor = GetComponentInParent<CharacterActor>();
        CharacterStateController = this.transform.parent.GetComponentInChildren<CharacterStateController>();
    }

#warning(����û��)
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

            // ���� NormalMovement ʵ���ı���
            if (normalMovementState != null)
            {

                return normalMovementState.moving;
                // ʹ�� normalMovementState ����ʵ���ı���
                // ����: normalMovementState.SomeVariable
            }
        }
        return false;
    }
}
