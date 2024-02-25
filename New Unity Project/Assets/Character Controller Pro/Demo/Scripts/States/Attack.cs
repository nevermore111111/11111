using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
using Rusk;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 
/// <summary>
public class Attack : CharacterState
{
    #region(攻击数据)
    public bool isAttack
    {
        get
        { return CharacterActor.CharacterInfo.attackAndDefendInfo.isAtttack; }
        set
        { CharacterActor.CharacterInfo.attackAndDefendInfo.isAtttack = value; }
    }
    public int combo
    {
        get
        { return CharacterActor.CharacterInfo.attackAndDefendInfo.combo; }
        set
        { CharacterActor.CharacterInfo.attackAndDefendInfo.combo = value; }

    }
    public bool canInput
    {
        get
        { return CharacterActor.CharacterInfo.attackAndDefendInfo.canInput; }
        set
        { CharacterActor.CharacterInfo.attackAndDefendInfo.canInput = value; }

    }
    public bool isJustEnter
    {
        get
        { return CharacterActor.CharacterInfo.attackAndDefendInfo.isJustEnter; }
        set
        { CharacterActor.CharacterInfo.attackAndDefendInfo.isJustEnter = value; }

    }
    public bool canChangeState
    {
        get
        { return CharacterActor.CharacterInfo.attackAndDefendInfo.canChangeState; }
        set
        { CharacterActor.CharacterInfo.attackAndDefendInfo.canChangeState = value; }

    }
    // protected  GameObject selectEnemy;
    public int MaxCombo
    {
        get
        { return CharacterActor.CharacterInfo.attackAndDefendInfo.maxCombo; }
        set
        { CharacterActor.CharacterInfo.attackAndDefendInfo.maxCombo = value; }

    }
    public AttackMode currentAttackMode
    {
        get
        { return CharacterActor.CharacterInfo.attackAndDefendInfo.attackMode; }
        set
        { CharacterActor.CharacterInfo.attackAndDefendInfo.attackMode = value; }
    }
    #endregion
    //这个是范围内的敌人，利用一个球判定进入范围的敌人，进入了就添加在名单里面；
    // public static List<GameObject> enemys = new List<GameObject>();
    //这个onceAttack是用来判定每次攻击只执行一次动画减慢效果
    private NormalMovement NormalMovement;

    [SerializeField]
    //进入attack状态时的体型
    public Vector2 targetAttackWidthAndHeigh;
    private Vector2 normalHeightAndWidth;
    public Attack attack;
    protected WeaponManager[] weaponManagers 
    {
        get => CharacterActor.CharacterInfo.attackAndDefendInfo.weaponManagers;
    }
    public bool useGravity
    {
        get
        { return CharacterActor.CharacterInfo.attackAndDefendInfo.useGravity; }
        set
        { CharacterActor.CharacterInfo.attackAndDefendInfo.useGravity = value; }
    }
    public static float AttackGravity = 10f;
    public float executeDis = 3f;
    //技能攻击时，如果有敌人在面前时，最大转向角度。
    public float maxAutoAnglerotate = 45f;
    //攻击时，敌人不在前方，攻击时的最大转向角度
    public float maxAttackAngleNoenemy = 20f;
    //10 地面普通攻击 剑
    //11 剑下落攻击 剑
    //12 地面击飞  拳
    //13 空中sp  拳
    public static bool isNextAttack = false;
    public float CharacterAttackDistance = 1.8f;

    //public List<IKPar> IKPar;


    /// <summary>
    /// false是展示
    /// </summary>
    /// <param name="ExitAttack"></param>
    public void ChangeWeaponState(bool ExitAttack)
    {
        if (ExitAttack == true)
        {
            foreach (var weapon in weaponManagers)
            {
                weapon.gameObject.SetActive(false);
            }
        }
        else
        {
            switch (currentAttackMode)
            {
                case AttackMode.AttackOnGround:
                    {
                        #region(学习)
                        /*
                         * list.foreach()//遍历这个列表中的所有物体，并且对其进行某种操作
                         */
                        #endregion
                        foreach (var weapon in weaponManagers)
                        {
                            weapon.gameObject.SetActive(true);
                        }
                        weaponManagers.Where(_ => _.kind != WeaponKind.sword).ToList().ForEach(_ => _.gameObject.SetActive(false));
                        break;
                    }
                case AttackMode.AttackOnGround_fist:
                    {
                        foreach (var weapon in weaponManagers)
                        {
                            weapon.gameObject.SetActive(true);
                        }
                        weaponManagers.Where(_ => _.kind != WeaponKind.fist).ToList().ForEach(_ => _.gameObject.SetActive(false));
                        break;
                    }
            }
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentAttackMode = AttackMode.AttackOnGround;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentAttackMode = AttackMode.AttackOnGround_fist;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {

        }
    }

    protected override void Awake()
    {
        targetAttackWidthAndHeigh = new(1f, 1.58f);
        //ResettargetAttackWidthAndHeigh(new Vector2(1.5f,1.58f));
        attack = GetComponent<Attack>();

        base.Awake();
        NormalMovement = GetComponent<NormalMovement>();
    }
    protected override void Start()
    {
        base.Start();
        normalHeightAndWidth = CharacterActor.BodySize;

    }
    public override void EnterBehaviour(float dt, CharacterState fromState)
    {
        //进入攻击时修改人物的高度和宽
        //几率并且修改
        //HeighAndWidth = CharacterActor.BodySize;
        base.EnterBehaviour(dt, fromState);
        isAttack = true;
        useGravity = false;
        canInput = false;
        if (CharacterStateController.PreviousState is not StartPlay)
            isJustEnter = true;
        //暂时记一下，在设置宽度为1.5f的时候
        CharacterActor.CheckAndSetSize(targetAttackWidthAndHeigh);

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
        CharacterActor.Animator.SetBool("attack", false);
        CharacterActor.Animator.speed = 1f;
        CharacterActor.SetUpRootMotion(false, false);
        CharacterActor.CheckAndSetSize(normalHeightAndWidth, Lightbug.CharacterControllerPro.Core.CharacterActor.SizeReferenceType.Bottom);
        // CharacterActor.SetSize(HeighAndWidth,)
        //DOTween.Kill("AttackSizeChange");
    }

    public override void UpdateBehaviour(float dt)
    {
        if (!canPlayerControl)
        {
            return;
        }
        if (isActiveBaseAutoHandleVelocity)
        {
            BaseProcessVelocity(dt);
        }
        SetCombo();
    }
    /// <summary>
    /// 根据输入，来确认当前的combo
    /// </summary>
    protected void SetCombo()
    {

        if (CharacterActions.spAttack.value)
        {
            if (CharacterActor.IsGrounded)
            {
                //if(currentAttackMode == AttackMode.AttackOnGround)
                {
                    isNextAttack = true;
                    SpAttack = 10;
                }
            }
            else if (canAttackInair)//&& currentAttackMode == AttackMode.AttackOnGround)
            {
                isNextAttack = true;
                SpAttack = 11;
            }
        }
        if (CharacterActions.attack.value)
        {
            //按下攻击键位
            if (canInput)
            {
                if (canExecute())
                {
                    //执行处决，先冲过去
                    executeStart();
                }
                else
                {
                    canInput = false;
                    isNextAttack = true;
                    combo++;
                    if (combo > MaxCombo)
                    {
                        combo = 1;
                    }
                    CharacterActor.Animator.SetInteger("combo", combo);
                }
            }
        }

    }

    protected virtual void executeStart()
    {
        canInput = false;
        isNextAttack = true;
        combo = 0;
    }
    protected bool canExecute()
    {
        if (CharacterActor.CharacterInfo.selectEnemy != null && CharacterActor.CharacterInfo.selectEnemy.canBeExecuted == true)
        {
            //距离够近且自身在地面
            if (CheckDis(attack.executeDis) && CharacterActor.IsGrounded)
            {
                //
                return true;

            }
        }
        return false;
    }

    private bool CheckDis(float distance)
    {
        return (CharacterActor.CharacterInfo.transform.position - CharacterActor.CharacterInfo.selectEnemy.transform.position).magnitude < distance;
    }

    protected void UseGravity(float dt)
    {
        if (!CharacterActor.IsStable)
            CharacterActor.VerticalVelocity += CustomUtilities.Multiply(-CharacterActor.Up, AttackGravity, dt);
    }
    public override void CheckExitTransition()
    {
        base.CheckExitTransition();
        if (!canPlayerControl)
        {
            return;
        }
        else if (CharacterActions.jump.value)
        {
            CharacterActor.ForceNotGrounded();
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
        else if (CharacterActor.CharacterInfo.ToEvade)
        {
            CharacterStateController.EnqueueTransition<Evade>();
        }
        else if (CharacterActor.IsStable && CharacterActions.defend.value && !isAttack)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
    }
    /// <summary>
    /// 开局设置这个有问题不知道为什么
    /// </summary>
    private async void ResettargetAttackWidthAndHeigh(Vector2 target)
    {
        await UniTask.Delay(1000);
        Debug.Log("设置了");
        targetAttackWidthAndHeigh = target;
    }
}
