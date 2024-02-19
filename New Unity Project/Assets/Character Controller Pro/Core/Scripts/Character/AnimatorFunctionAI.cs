using Cinemachine;
using Cinemachine.Utility;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 
/// <summary>
public class AnimatorFunctionAI : MonoBehaviour
{
    [SerializeField]
    //private MainCharacter mainCharacter;
    private CharacterInfo _characterInfo;
    //private WeaponManager weaponManager;
    private AIAttack aIAttack;
    //private Attack Attack;
    CharacterStateController CharacterStateController;
    public CinemachineFreeLook CinemachineFreeLook;
    private List<WeaponManager> weaponManagers;//这个是所有武器
    private static AnimationConfig animationConfig;//这个是动画参数
    public SoloAnimaConfig CurrentAnimConfig;//这个是用来记录单个时间的参数
    public int currentHitIndex;//当前这个攻击的第n次攻击检测
    public int hitKind;//现在攻击的种类
    public string activeWeaponDetect;//现在激活的碰撞区域
    public string currentStateName;//当前正在播放的动画
    public WeaponData WeaponData;
    CharacterActor characterActor;

    private TimelineManager timelineManager;

    private void Awake()
    {
        weaponManagers = GetComponentsInChildren<WeaponManager>().ToList();
        aIAttack = transform.parent.parent.GetComponentInChildren<AIAttack>();
        CharacterStateController = transform.parent.parent.GetComponentInChildren<CharacterStateController>();
        timelineManager = GetComponent<TimelineManager>();
        WeaponData = FindAnyObjectByType<WeaponData>();
        characterActor = GetComponentInParent<CharacterActor>();
        //
    }
    private void Start()
    {
        animationConfig = DataLoad.Instance.animationConfig;
    }


    public void AttackEnd()
    {
        characterActor.Animator.speed = 1f;
        if (!characterActor.Animator.IsInTransition(0))
        {
            if (CharacterStateController.CurrentState is AIAttack && !DonotUseRootmovtion())
            {
                characterActor.SetUpRootMotion(true, true);
            }
            aIAttack.isAttack = false;
            SetWeaponDetection(false);
        }
    }
    public void HitEnd()
    {
        characterActor.Animator.speed = 1f;
        if (!characterActor.Animator.IsInTransition(0))
        {
            SetWeaponDetection(false);
        }
    }

    public void HitStart()//int hitKind, string activeWeaponDetect
    {
        SetWeaponDirection();
        SetStrengthAndDetector();
        //开启武器检测，并且开启对应的碰撞区域
        ActiveDetectionByStringPar(activeWeaponDetect, SetWeaponDetection(true));
        //foreach (var manager in weaponManagers)
        //{
        //    if (manager.isActiveAndEnabled)
        //    {
        //        manager.ToggleDetection(true);
        //        if (manager != null)
        //        {
        //            //根据动画参数激活对应的碰撞区域
        //            manager.weaponFx = CurrentAnimConfig.HittedEffect;
        //            ActiveDetectionByStringPar(activeWeaponDetect, manager);

        //        }
        //        break;
        //    }
        //}
        currentHitIndex++;
    }

    


    public void HitReStart()//int Hit = 1, string activeWeaponDetect = null
    {
        SetWeaponDirection();

        SetStrengthAndDetector();
        foreach (var manager in weaponManagers)
        {
            if (manager.isActiveAndEnabled)
            {
                if (activeWeaponDetect != null)
                {
                    ActiveDetectionByStringPar(activeWeaponDetect, manager);
                }
                manager.ToggleDetection(false);
                manager.ToggleDetection(true);
                break;
            }
        }
        currentHitIndex++;
    }


    /// <summary>
    /// 攻击开始
    /// </summary>
    /// <param name="attackName"></param>
    public void AttackStart(string attackName)
    {
        //修正当前的攻击方向
        characterActor.Animator.speed = 1f;
        characterActor.UseRootMotion = true;
        //更新攻击名称和配置表
        ResetCurrentInfo(attackName);

        //开始一次攻击
        //Attack.isNextAttack = false;//这个代表已经执行了下一次攻击
        aIAttack.isAttack = true;
        //characterActor.Animator.SetBool("attack", true);
        aIAttack.canChangeState = false;
        //characterActor.Animator.SetInteger("specialAttack", 0);
        ResetAttackRootAndrotate();
    }

















    //_______________________________________________非动画事件分割线_______________________________________________
    //_______________________________________________非动画事件分割线_______________________________________________
    //_______________________________________________非动画事件分割线_______________________________________________

    /// <summary>
    /// 是否要自动更新武器方向，如果不自动，回去读表。attack开始时一定去开自动
    /// </summary>
    /// <param name="autoUpdate"></param>
    private void SetWeaponDirection(bool autoUpdate = false)
    {
        if (autoUpdate)
        {
            foreach (WeaponManager manager in weaponManagers)
            {
                if (manager.isActiveAndEnabled)
                {
                    manager.isNeedUpdateDirection = true;
                    //weaponManager = manager;
                    return;
                }
            }
        }
        //修正当前的攻击方向
        if (CurrentAnimConfig.AttackDirection.Length < 3)
        {
            //这代表没填写
            //要继续更新
            foreach (WeaponManager manager in weaponManagers)
            {
                if (manager.isActiveAndEnabled)
                {
                    manager.isNeedUpdateDirection = true;
                    //weaponManager = manager;
                    break;
                }

            }
        }
        else
        {
            //会根据当前此次攻击的次数去区对应的攻击
            foreach (WeaponManager manager in weaponManagers)
            {
                if (manager.isActiveAndEnabled)
                {
                    //weaponManager = manager;
                    manager.isNeedUpdateDirection = false;
                    Vector3 DirectionIncharacter = new Vector3(CurrentAnimConfig.AttackDirection[currentHitIndex * 3], CurrentAnimConfig.AttackDirection[currentHitIndex * 3 + 1], CurrentAnimConfig.AttackDirection[currentHitIndex * 3 + 2]);
                    //然后我需要把这个
                    manager.WeaponWorldDirection = aIAttack.transform.TransformDirection(DirectionIncharacter);//这个时候已经转化成世界方向了
                    break;
                }
            }
        }
    }

    private float originalSpeed;
    /// <summary>
    /// 开启或者关闭当前武器的检测,一般情况下，只有一个开启，否则就是错了！
    /// </summary>
    /// <param name="IsOpenManagerToggle"></param>
    private WeaponManager SetWeaponDetection(bool IsOpenManagerToggle)
    {
        foreach (var manager in weaponManagers)
        {
            if (manager.isActiveAndEnabled)
            {
                manager.ToggleDetection(false);
                return manager;
            }
        }
        return null;
    }
    /// <summary>
    /// 写入当前的攻击力度和开启的攻击区域
    /// </summary>
    private void SetStrengthAndDetector()
    {
        if (CurrentAnimConfig.HitStrength.Length > currentHitIndex)
            hitKind = CurrentAnimConfig.HitStrength[currentHitIndex];
        if (CurrentAnimConfig.HitDetect.Length > currentHitIndex)
            activeWeaponDetect = CurrentAnimConfig.HitDetect[currentHitIndex];
        _characterInfo.HitStrength = hitKind;
    }

    /// <summary>
    ///根据动画时间传递的参数来确认当前应该激活的检测区域
    /// </summary>
    /// <param name="activeWeaponDetect"></param>
    /// <param name="manager"></param>
    private void ActiveDetectionByStringPar(string activeWeaponDetect, WeaponManager manager)
    {
        int[] weaponIndexes = ConvertStringToIntArray(activeWeaponDetect);
        manager.ActiveWeaponDetectors = weaponIndexes.Select(index => (WeaponDetector)index).ToArray();
    }

    private int[] ConvertStringToIntArray(string str)
    {
        int[] intArray = new int[str.Length];
        for (int i = 0; i < str.Length; i++)
        {
            intArray[i] = int.Parse(str[i].ToString());
        }
        return intArray;
    }
    /// <summary>
    /// 如果没有敌人就正常攻击，有的话会转向敌人，并且会根据自身距离敌人的距离决定是否开启rootmotion
    /// </summary>
    private void ResetAttackRootAndrotate()
    {

        if (_characterInfo.enemies.Count != 0)
        {
            //新语法
            //GameObject[] gamesEnemy = mainCharacter.enemies.Select(m => m.gameObject).ToArray();
            SetActorForword();
            //Debug.Log(Attack.CharacterActor.Forward);

            if (DonotUseRootmovtion())
            {
                characterActor.SetUpRootMotion(false, false);
                if (characterActor.RigidbodyComponent.IsKinematic == false)
                    characterActor.PlanarVelocity = Vector3.zero;
            }
        }
        else
        {
            //没有单位就可以自由转向，但是只能在攻击开始的时候转向
            characterActor.Forward = CharacterStateController.InputMovementReference;
        }
    }

    private bool DonotUseRootmovtion()
    {
        return (transform.position - _characterInfo.selectEnemy.transform.position).magnitude < aIAttack.CharacterAttackDistance;
    }

    /// <summary>
    /// 设置人物正方向
    /// </summary>
    /// <param name="gamesEnemy"></param>
    private void SetActorForword()
    {
        bool needAutoRotate = false;
        if (characterActor.CharacterInfo.enemies.Count > 0)
        {
            //选择的鱼类是最近的鱼类
            _characterInfo.selectEnemy = characterActor.CharacterInfo.enemies.OrderBy((_) => (_.transform.position - _characterInfo.transform.position).sqrMagnitude).First();
            needAutoRotate = true;
        }
        else if (CharacterStateController.InputMovementReference != Vector3.zero)
        {
            //否则去按输入转向，定义转向速度和时间。
            Vector3 target = CharacterStateController.InputMovementReference.ProjectOntoPlane(Vector3.up).normalized;//我想转向的方向
            Quaternion targetQua = Quaternion.LookRotation(target, Vector3.up);
            DOTween.To(() => 0f, value =>
            {
                //旋转一定角度
                Quaternion.RotateTowards(characterActor.transform.rotation, targetQua, 0.1f * Time.deltaTime);
            }, 1f, 0.1f);
            //DOTween.To();//执行

        }
        //选择的敌人不为0，自动去选择攻击单位
        if (needAutoRotate)
        {
            //我应该面向的方向
            Vector3 targetMainCharacterForward = (_characterInfo.selectEnemy.transform.position - _characterInfo.transform.position).ProjectOntoPlane(Vector3.up).normalized;
            characterActor.transform.rotation = Quaternion.LookRotation(targetMainCharacterForward, Vector3.up);
        }


    }

    /// <summary>
    /// 重新根据动画名称更新配置信息
    /// </summary>
    /// <param name="attackName"></param>
    private void ResetCurrentInfo(string attackName)
    {
        //如果名字一致不做任何事情
        if (currentStateName == attackName)
        {
            currentStateName = attackName;
            GetAnimationPar(currentStateName);//根据当前的动画传入的state去拿动画参数
            timelineManager.PlayTimelineByName(CurrentAnimConfig.ClipName); // 播放对应名称的Playable
        }
        else
        {
            currentStateName = attackName;
            GetAnimationPar(currentStateName);//根据当前的动画传入的state去拿动画参数
            timelineManager.PlayTimelineByName(CurrentAnimConfig.ClipName); // 播放对应名称的Playable
        }
        currentHitIndex = 0;
    }


    private void GetAnimationPar(string currentStateName)
    {

        int index = FindStateIndexByName(currentStateName);//根据当前的动画名称来修改
        if (index != -1)
        {
            CurrentAnimConfig = new SoloAnimaConfig(
                animationConfig.Index[index],
                animationConfig.ClipName[index],
                animationConfig.Combo[index],
                animationConfig.AnmationStateName[index],
                animationConfig.HitStrength[index],
                animationConfig.HitDetect[index],
                animationConfig.AnimStateInfo[index],
                animationConfig.SpAttackPar[index],
                animationConfig.AttackDirection[index],
                animationConfig.HittedEffect[index]
            );
        }
        else
        {
            Debug.LogError($"没找到{currentStateName}");
        }
    }


    private int FindStateIndexByName(string stateName)
    {
        if (stateName != null)
        {
            for (int i = 0; i < animationConfig.AnmationStateName.Count; i++)
            {
                if (animationConfig.AnmationStateName[i] == stateName)
                {
                    return i;
                }
            }
        }
        return -1;
    }



}
