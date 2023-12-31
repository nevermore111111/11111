using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
using Rusk;
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
    public static AttackMode currentAttackMode = AttackMode.AttackOnGround;
    [SerializeField]
    private Vector2 HeighAndWidth;
    private Vector2 normalHeightAndWidth;
    public Attack attack;
    protected WeaponManager[] weaponManagers;
    public static bool useGravity = false;
    public static float AttackGravity = 10f;
   
    //10 地面普通攻击 剑
    //11 剑下落攻击 剑
    //12 地面击飞  拳
    //13 空中sp  拳
    public static bool isNextAttack = false;

    public enum AttackMode
    {
        AttackOnGround,
        //AttackOffGround,
        AttackOnGround_fist
    }
    /// <summary>
    /// false是展示
    /// </summary>
    /// <param name="ExitAttack"></param>
    public void ChangeWeaponState(bool ExitAttack)
    {
        if(ExitAttack == true)
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
                        //foreach (var weapon in weaponManagers)
                        //{
                        //    weapon.gameObject.SetActive(true);
                        //}
                        // //如果是使用fist攻击，那么只要关闭当前的weapon检测就可以了
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
        weaponManagers = this.transform.parent.GetComponentsInChildren<WeaponManager>();
        HeighAndWidth = new(1f, 1.58f);
        attack = GetComponent<Attack>();
        OnceAttack = false;
        base.Awake();
        MaxCombo = 1;
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
        CharacterActor.CheckAndSetSize(HeighAndWidth, Lightbug.CharacterControllerPro.Core.CharacterActor.SizeReferenceType.Bottom);

       // //根据当前进入的类，去调整当前的timeline的数量
       // string className = this.GetType().Name;
       // //当进入对应模式的时候，去切换对应的timeline数组
       //timelineManager.SwapTimelinesByAssetName(className);
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
    }

    public override void UpdateBehaviour(float dt)
    {
        if (!canPlayerControl)
        {
            return;
        }
        if (useGravity)
        {
            UseGravity(dt);
        }
        SetCombo();
    }
    /// <summary>
    /// 根据输入，来确认当前的combo
    /// </summary>
    private void SetCombo()
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
            else if(canAttackInair )//&& currentAttackMode == AttackMode.AttackOnGround)
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
                isNextAttack = true;
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

    private void UseGravity(float dt)
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
        if (CharacterActions.jump.value)
        {
            CharacterActor.ForceNotGrounded();
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
        if (NormalMovement.CanEvade())
        {
            CharacterStateController.EnqueueTransition<Evade>();
        }
    }
}
