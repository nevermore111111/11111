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
    //����Ƿ�Χ�ڵĵ��ˣ�����һ�����ж����뷶Χ�ĵ��ˣ������˾�������������棻
    // public static List<GameObject> enemys = new List<GameObject>();
    //���onceAttack�������ж�ÿ�ι���ִֻ��һ�ζ�������Ч��
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
   
    //10 ������ͨ���� ��
    //11 �����乥�� ��
    //12 �������  ȭ
    //13 ����sp  ȭ
    public static bool isNextAttack = false;

    public enum AttackMode
    {
        AttackOnGround,
        //AttackOffGround,
        AttackOnGround_fist
    }
    /// <summary>
    /// false��չʾ
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
                        #region(ѧϰ)
                        /*
                         * list.foreach()//��������б��е��������壬���Ҷ������ĳ�ֲ���
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
                        // //�����ʹ��fist��������ôֻҪ�رյ�ǰ��weapon���Ϳ�����
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
        //���빥��ʱ�޸�����ĸ߶ȺͿ�
        //���ʲ����޸�
        //HeighAndWidth = CharacterActor.BodySize;
        base.EnterBehaviour(dt, fromState);
        isAttack = true;
        useGravity = false;
        canInput = false;
        if (CharacterStateController.PreviousState is not StartPlay)
            isJustEnter = true;
        CharacterActor.CheckAndSetSize(HeighAndWidth, Lightbug.CharacterControllerPro.Core.CharacterActor.SizeReferenceType.Bottom);

       // //���ݵ�ǰ������࣬ȥ������ǰ��timeline������
       // string className = this.GetType().Name;
       // //�������Ӧģʽ��ʱ��ȥ�л���Ӧ��timeline����
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
    /// �������룬��ȷ�ϵ�ǰ��combo
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
            //���¹�����λ
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
