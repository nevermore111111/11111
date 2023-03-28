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
    //这个是范围内的敌人，利用一个球判定进入范围的敌人，进入了就添加在名单里面；
   // public static List<GameObject> enemys = new List<GameObject>();
    //这个onceAttack是用来判定每次攻击只执行一次动画减慢效果
    public static bool OnceAttack;
    private NormalMovement NormalMovement;
   


    protected override void Awake()
    {
        OnceAttack = false;
        Debug.Log("attack初始化");
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
        //进入攻击时修改人物的高度和宽
        //几率并且修改
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
