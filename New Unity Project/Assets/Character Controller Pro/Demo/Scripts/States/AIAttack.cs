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

    public bool attackEnd;

    public enum AttackMode
    {
        AttackOnGround,
        AttackOffGround,
        AttackOnGround_fist
    }

    private void Update()
    {

        
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
        Debug.Log("¿ªÊ¼");
    }
    public override void ExitBehaviour(float dt, CharacterState toState)
    {
        if(attackEnd)
        {

        }
    }

    public override void UpdateBehaviour(float dt)
    {

    }
    public override void CheckExitTransition()
    {
        if (attackEnd)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
    }
}
