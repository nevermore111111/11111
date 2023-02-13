using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// <summary>
public class Attack : CharacterState
{
   
    protected static bool isAttack;
    protected static int combo;
    protected static bool canInput;
    protected static bool isJustEnter;
    protected static bool canChangeState;
    protected static GameObject selectEnemy;
    public static int MaxCombo;
    //����Ƿ�Χ�ڵĵ��ˣ�����һ�����ж����뷶Χ�ĵ��ˣ������˾�������������棻
    public static List<GameObject> enemys;
    
    protected override void Awake()
    {
       
        selectEnemy = null;
        base.Awake();
        MaxCombo = 1;
        enemys = new List<GameObject>();
    }
    protected override void Start()
    {
        base.Start();
    }
    public override void EnterBehaviour(float dt, CharacterState fromState)
    {
        //���빥��ʱ�޸�����ĸ߶ȺͿ�
        //���ʲ����޸�
        //HeighAndWidth = CharacterActor.BodySize;
        base.ExitBehaviour(dt, fromState);
        isAttack = true;
        canInput = false;
        isJustEnter = true;
    }
    public override void ExitBehaviour(float dt, CharacterState toState)
    {
        base.ExitBehaviour(dt, toState);
        isJustEnter = true;
       // CharacterActor.SetSize(HeighAndWidth,)
    }

    public override void UpdateBehaviour(float dt)
    {
        
    }
    public override void CheckExitTransition()
    {
        base.CheckExitTransition();
        if(CharacterActions.jump.value)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
        
    }
}
