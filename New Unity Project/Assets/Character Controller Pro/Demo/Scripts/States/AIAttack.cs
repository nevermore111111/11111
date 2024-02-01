using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using Rusk;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// 
/// <summary>
public class AIAttack : CharacterState
{

    public List<AIAttackData> attacks;
    public bool isAttacking;
    public enum AIAttackState 
    {
        chooseAttack,//选择攻击
        attack,
        attackEnd
    }

    public enum AttackMode
    {
        AttackOnGround,
        AttackOffGround,
        AttackOnGround_fist
    }


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
        Debug.Log("开始");
    }
    public override void ExitBehaviour(float dt, CharacterState toState)
    {

    }

    public override void UpdateBehaviour(float dt)
    {

    }
    public override void CheckExitTransition()
    {
        if(CharacterActor.IsGrounded) 
        {

        }
        else//不在地面的时候直接进入normalmovement
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
    }
}
[Serializable]
public class AIAttackData 
{
    public string attackName;
    public int attackChooseWeight;
}
