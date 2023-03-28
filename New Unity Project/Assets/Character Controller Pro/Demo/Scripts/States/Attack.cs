using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using Rusk;
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
   // protected  GameObject selectEnemy;
    public static int MaxCombo;
    //����Ƿ�Χ�ڵĵ��ˣ�����һ�����ж����뷶Χ�ĵ��ˣ������˾�������������棻
   // public static List<GameObject> enemys = new List<GameObject>();
    //���onceAttack�������ж�ÿ�ι���ִֻ��һ�ζ�������Ч��
    public static bool OnceAttack;
    private NormalMovement NormalMovement;
   


    protected override void Awake()
    {
        OnceAttack = false;
        Debug.Log("attack��ʼ��");
        base.Awake();
        MaxCombo = 1;
        NormalMovement = GetComponent<NormalMovement>();
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
        base.EnterBehaviour(dt, fromState);
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
        if (CharacterActions.jump.value)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
        if(NormalMovement.CanEvade())
        {
            CharacterStateController.EnqueueTransition<Evade>();
        }
    }
}
