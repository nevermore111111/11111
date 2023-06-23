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

    public static bool isAttack;
    public static int combo;
    public static bool canInput;
    public static bool isJustEnter;
    public static bool canChangeState;
   // protected  GameObject selectEnemy;
    public static int MaxCombo;
    //这个是范围内的敌人，利用一个球判定进入范围的敌人，进入了就添加在名单里面；
   // public static List<GameObject> enemys = new List<GameObject>();
    //这个onceAttack是用来判定每次攻击只执行一次动画减慢效果
    public static bool OnceAttack;
    private NormalMovement NormalMovement;
    public AttackMode currentAttackMode = AttackMode.AttackOnGround;
    [SerializeField]
    private Vector2 HeighAndWidth;
    private Vector2 normalHeightAndWidth;
    public Attack attack;
    TimelineManager timelineManager;
    WeaponManager[] weaponManagers;


    public enum AttackMode
    {
        AttackOnGround,
        AttackOffGround,
        AttackOnGround_fist
    }

    
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentAttackMode = AttackMode.AttackOnGround;
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentAttackMode = AttackMode.AttackOffGround;
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentAttackMode = AttackMode.AttackOnGround_fist;
           
        }
    }

    protected override void Awake()
    {
        weaponManagers = GetComponentsInChildren<WeaponManager>();
        HeighAndWidth =  new(1f, 1.58f);
        attack = GetComponent<Attack>();
        OnceAttack = false;
        Debug.Log("attack初始化");
        base.Awake();
        MaxCombo = 1;
        NormalMovement = GetComponent<NormalMovement>();
    }
    protected override void Start()
    {
        base.Start();
        normalHeightAndWidth = CharacterActor.BodySize;
        timelineManager = this.transform.parent.gameObject.GetComponentInChildren<TimelineManager>();
    }
    public override void EnterBehaviour(float dt, CharacterState fromState)
    {
        //进入攻击时修改人物的高度和宽
        //几率并且修改
        //HeighAndWidth = CharacterActor.BodySize;
        base.EnterBehaviour(dt, fromState);
        isAttack = true;

        canInput = false;
        if(CharacterStateController.PreviousState is  not StartPlay)
        isJustEnter = true;
        CharacterActor.CheckAndSetSize(HeighAndWidth, Lightbug.CharacterControllerPro.Core.CharacterActor.SizeReferenceType.Bottom);

        //根据当前进入的类，去调整当前的timeline的数量
        string className = this.GetType().Name;
        //当进入对应模式的时候，去切换对应的timeline数组
        timelineManager.SwapTimelinesByAssetName(className);
    }
    public override void ExitBehaviour(float dt, CharacterState toState)
    {
        foreach (var manager in weaponManagers)
        {
            manager.isOnDetection = false;
            manager.ToggleDetection(false);
        }
        base.ExitBehaviour(dt, toState);
        isJustEnter = true;
        isAttack = false;
        CharacterActor.CheckAndSetSize(normalHeightAndWidth, Lightbug.CharacterControllerPro.Core.CharacterActor.SizeReferenceType.Bottom);
        // CharacterActor.SetSize(HeighAndWidth,)
    }

    public override void UpdateBehaviour(float dt)
    {
        
    }
    public override void CheckExitTransition()
    {
        base.CheckExitTransition();
        if (!canPlayerControl)
        {
            return;
        }
        if (CharacterActions.jump.value)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
        if(NormalMovement.CanEvade())
        {
            CharacterStateController.EnqueueTransition<Evade>();
        }
        if (currentAttackMode == AttackMode.AttackOffGround)
        {
            //CharacterStateController.EnqueueTransition<>();
            return;
        }
    }
}
