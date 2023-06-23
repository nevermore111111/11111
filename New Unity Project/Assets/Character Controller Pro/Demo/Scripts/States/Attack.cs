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
    //����Ƿ�Χ�ڵĵ��ˣ�����һ�����ж����뷶Χ�ĵ��ˣ������˾�������������棻
   // public static List<GameObject> enemys = new List<GameObject>();
    //���onceAttack�������ж�ÿ�ι���ִֻ��һ�ζ�������Ч��
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
        Debug.Log("attack��ʼ��");
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
        //���빥��ʱ�޸�����ĸ߶ȺͿ�
        //���ʲ����޸�
        //HeighAndWidth = CharacterActor.BodySize;
        base.EnterBehaviour(dt, fromState);
        isAttack = true;

        canInput = false;
        if(CharacterStateController.PreviousState is  not StartPlay)
        isJustEnter = true;
        CharacterActor.CheckAndSetSize(HeighAndWidth, Lightbug.CharacterControllerPro.Core.CharacterActor.SizeReferenceType.Bottom);

        //���ݵ�ǰ������࣬ȥ������ǰ��timeline������
        string className = this.GetType().Name;
        //�������Ӧģʽ��ʱ��ȥ�л���Ӧ��timeline����
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
