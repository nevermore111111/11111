using Cinemachine;
using Cysharp.Threading.Tasks;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Implementation;
using MagicaCloth2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 
/// <summary>
public class AnimatorFunction : MonoBehaviour
{
    [SerializeField]
    private MainCharacter mainCharacter;
    private WeaponManager weaponManager;
    private Attack Attack;
    CharacterStateController CharacterStateController;
    public CinemachineFreeLook CinemachineFreeLook;
    CameraEffects CameraEffects;
    private List<WeaponManager> weaponManagers;//这个是所有武器
    private AnimationConfig animationConfig;//这个是动画参数
    public SoloAnimaConfig CurrentAnimConfig;//这个是用来记录单个时间的参数
    public int currentHitIndex;//当前这个攻击的第n次攻击检测
    private int currentAnimPar;
    public int hitKind;//现在攻击的种类
    public string activeWeaponDetect;//现在激活的碰撞区域
    public string currentStateName;//当前正在播放的动画
    public WeaponData WeaponData;

    private TimelineManager timelineManager;
    //private Action<int> hitActionOfImpulse;
    //private Action<int> hitActionOfPlayFX;



    private void Awake()
    {
        weaponManagers = GetComponentsInChildren<WeaponManager>().ToList();
        Attack = transform.parent.parent.GetComponentInChildren<Attack>();
        CharacterStateController = transform.parent.parent.GetComponentInChildren<CharacterStateController>();
        //CameraEffects = CinemachineFreeLook.GetComponent<CameraEffects>();
        timelineManager = GetComponent<TimelineManager>();
        WeaponData = FindAnyObjectByType<WeaponData>();
        //
    }
    private void Start()
    {
        animationConfig = FindAnyObjectByType<DataLoad>().animationConfig;
    }
    public void JumpStart()
    {
        //关闭动画机进入的条件
        Attack.CharacterActor.Animator.SetBool("jump", false);
    }

    public void Idle()
    {
        Attack.isAttack = false;
        Attack.CharacterActor.Animator.SetBool("attack", false);
        Attack.combo = 0;
        Attack.CharacterActor.Animator.SetInteger("combo", Attack.combo);
        Attack.canInput = true;
        Attack.canChangeState = true;
    }

    public void Fly()
    {
        Attack.CharacterActor.alwaysNotGrounded = true;
    }
    public void FlyEnd()
    {
        Attack.CharacterActor.alwaysNotGrounded = false;
    }


    public void NormalIdle()
    {
        if (Attack.CharacterActor.Animator.IsInTransition(0) == false)
            Attack.CharacterActor.SetUpRootMotion(false, false);
    }
    public void Stop()
    {
        Attack.CharacterActor.Animator.SetBool(Attack.stopParameter, false);
    }

    public void AttackEnd()
    {
        Attack.CharacterActor.Animator.speed = 1f;
        if (!Attack.CharacterActor.Animator.IsInTransition(0))
        {
            if (CharacterStateController.CurrentState is Attack)
                Attack.CharacterActor.SetUpRootMotion(true, true);
            Attack.isAttack = false;
            Attack.CharacterActor.Animator.SetBool("attack", false);

            foreach (var manager in weaponManagers)
            {
                if (manager.isActiveAndEnabled)
                {
                    manager.ToggleDetection(false);
                    break;
                }
            }
        }
    }
    public void HitEnd()
    {
        Attack.CharacterActor.Animator.speed = 1f;
        if (!Attack.CharacterActor.Animator.IsInTransition(0))
        {
            foreach (var manager in weaponManagers)
            {
                if (manager.isActiveAndEnabled)
                {
                    manager.ToggleDetection(false);

                    break;
                }
            }
        }
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


    public void HitStart()//int hitKind, string activeWeaponDetect
    {
        Debug.LogError("开始攻击");
        Attack.CharacterActor.Animator.speed = 1.3f;
        hitKind = CurrentAnimConfig.HitStrength[currentHitIndex];
        activeWeaponDetect = CurrentAnimConfig.HitDetect[currentHitIndex];
        //设置当前攻击类别
        mainCharacter.HitStrength = hitKind;
        //根据当前攻击类别来进行
        //根据当前的detections进行调整这个激活的detection;
        currentHitIndex++;
        SetWeaponDirection();
        foreach (var manager in weaponManagers)
        {
            if (manager.isActiveAndEnabled)
            {
                manager.ToggleDetection(true);
                if (manager != null)
                {
                    //根据动画参数激活对应的碰撞区域
                    manager.weaponFx = CurrentAnimConfig.HittedEffect;
                    ActiveDetectionByStringPar(activeWeaponDetect, manager);

                    //switch (hitKind)
                    //{
                    //    case 0:
                    //        manager.AdjustFrequencyAndAmplitude(1, 0.5f);
                    //        break;
                    //    case 1:
                    //        manager.AdjustFrequencyAndAmplitude(1.5f, 0.4f);
                    //        break;
                    //    case 2:
                    //        manager.AdjustFrequencyAndAmplitude(2f, 0.4f);
                    //        break;
                    //    case 4:
                    //        manager.AdjustFrequencyAndAmplitude(3f, 0.4f);
                    //        break;
                    //    default:
                    //        manager.AdjustFrequencyAndAmplitude(1f, 1f);
                    //        break;
                    //}
                }
                break;
            }
        }

    }

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
                    weaponManager = manager;
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
                    weaponManager = manager;
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
                    weaponManager = manager;
                    manager.isNeedUpdateDirection = false;
                    Vector3 DirectionIncharacter = new Vector3(CurrentAnimConfig.AttackDirection[currentHitIndex * 3 - 3], CurrentAnimConfig.AttackDirection[currentHitIndex * 3 - 2], CurrentAnimConfig.AttackDirection[currentHitIndex * 3 - 1]);
                    //然后我需要把这个
                    manager.WeaponDirection = Attack.transform.TransformDirection(DirectionIncharacter);
                    break;
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        Debug.Log("绘制");
        if (weaponManager != null)
        {
            Debug.Log("开始绘制");
            Debug.Log(weaponManager.WeaponDirection);
            Gizmos.DrawLine(weaponManager.transform.position, weaponManager.transform.position + weaponManager.WeaponDirection);
        }

    }


    public void HitReStart()//int Hit = 1, string activeWeaponDetect = null
    {
        currentHitIndex++;
        SetWeaponDirection();
        mainCharacter.HitStrength = hitKind;
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

    public void SpAttackStart(string spAttackName)
    {
        AttackStart(spAttackName);
        //这里开始sp攻击，根据当前sp攻击的名称，设置当前spattack信息
        Attack.SpAttack = Mathf.RoundToInt(CurrentAnimConfig.SpAttackPar[0]);
        //根据当前的动画参数设置
    }

    /// <summary>
    /// 攻击开始
    /// </summary>
    /// <param name="attackName"></param>
    public void AttackStart(string attackName)
    {
        //修正当前的攻击方向

        Attack.CharacterActor.Animator.speed = 1f;
        Attack.CharacterActor.UseRootMotion = true;
        //更新攻击名称和配置表
        ResetCurrentInfo(attackName);
        //foreach(var weapon in weaponManagers)
        //{
        //    //
        //}
        //SetWeaponDirection(true);

        //开始一次攻击
        Attack.isNextAttack = false;//这个代表已经执行了下一次攻击
        Attack.isAttack = true;
        Attack.CharacterActor.Animator.SetBool("attack", true);
        Attack.canChangeState = false;
        Attack.OnceAttack = false;
        Attack.CharacterActor.Animator.SetInteger("specialAttack", 0);
        ResetAttackRootAndrotate();
    }

    /// <summary>
    /// 如果没有敌人就正常攻击，有的话会转向敌人，并且会根据自身距离敌人的距离决定是否开启rootmotion
    /// </summary>
    private void ResetAttackRootAndrotate()
    {
        
        if (mainCharacter.enemies.Count != 0)
        {
            //新语法
            GameObject[] gamesEnemy = mainCharacter.enemies.Select(m => m.gameObject).ToArray();
            mainCharacter.selectEnemy = HelpTools01.FindClosest(Attack.CharacterActor.gameObject, gamesEnemy).GetComponent<CharacterInfo>();
            Vector3 Forward = (mainCharacter.selectEnemy.transform.position - Attack.CharacterActor.transform.position).normalized;
            //Debug.Log(Attack.CharacterActor.Forward);
            Attack.CharacterActor.Forward = new(Forward.x, 0, Forward.z);
            Attack.CharacterActor.Up = Vector3.up;
            //Debug.Log(Attack.CharacterActor.Forward);
            if ((transform.position - mainCharacter.selectEnemy.transform.position).magnitude < 1.5f)
            {

                if (Attack.SpAttack == -1)
                {
                    Attack.CharacterActor.SetUpRootMotion(false, false);
                }
                Attack.CharacterActor.PlanarVelocity = Vector3.zero;
            }
        }
        else
        {
            //没有单位就可以自由转向，但是只能在攻击开始的时候转向
            Attack.CharacterActor.Forward = CharacterStateController.InputMovementReference;
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

    public void PlayTimeline(string TimelineName)
    {
        timelineManager.PlayTimelineByName(TimelineName);
    }

    public void CanGetInput()
    {
        Attack.CharacterActor.Animator.SetInteger("specialAttack", 0);
        Attack.canInput = true;
        Attack.SpAttack = -1;
    }

    public void Drop()
    {
        Attack.CharacterActor.UseRootMotion = false;
        Attack.useGravity = true;
        Attack.CharacterActor.VerticalVelocity -= 10f * Attack.CharacterActor.Up;
        Attack.CharacterActor.alwaysNotGrounded = false;
        //仅仅使用水平移动
    }
    public void CannotGetInput()
    {
        if (!Attack.isAttack)
        {
            //这样在攻击中的时候，不会被上一次的CannotGetInput重置combo
            Attack.combo = 0;
            Attack.CharacterActor.Animator.SetInteger("combo", 0);
        }
    }
    /// <summary>
    /// 玩家可以控制
    /// </summary>
    public void CanPlayerControl()
    {
        //如果之前的是，那么就开始操作。
        if (CharacterStateController.PreviousState is StartPlay)
        {
            CharacterState.canPlayerControl = true;
        }
    }





    /// <summary>
    /// 在一个连招开始时执行，定义这个连招的最大连击数量。
    /// </summary>
    public void ComboStart(int num)
    {
        Attack.MaxCombo = num;
        Attack.CharacterActor.Animator.SetInteger("specialAttack", 0);
    }
    public void SpAtk(int kind)
    {
        Attack.CharacterActor.Animator.SetInteger("specialAttack", kind);
    }
    public void End()
    {
        Attack.CharacterActor.Animator.SetTrigger("end");
    }






    //_______________________________________________非动画事件分割线_______________________________________________
    private float originalSpeed;

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


    public void SlowDownAnimator(float slowDownFactor, float duration)
    {
        originalSpeed = Attack.CharacterActor.Animator.speed; // 保存原始的播放速度
        Debug.Log(originalSpeed);

        Attack.CharacterActor.Animator.speed = originalSpeed * slowDownFactor; // 修改播放速度为当前的 slowDownFactor

        // 在指定的时间后恢复原始速度
        StartCoroutine(RestoreAnimatorSpeed(duration));
    }
    private System.Collections.IEnumerator RestoreAnimatorSpeed(float delay)
    {
        yield return new WaitForSeconds(delay);

        Attack.CharacterActor.Animator.speed = originalSpeed; // 恢复原始播放速度
    }

    private string GetPlayingClipName(Animator animator)
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
        {
            return clipInfo[0].clip.name;
        }
        return null;
    }
    private string GetTargetClipName(Animator animator)
    {
        AnimatorTransitionInfo transitionInfo = animator.GetAnimatorTransitionInfo(0);

        if (transitionInfo.anyState)//如果在过渡
        {
            int targetStateHash = transitionInfo.nameHash;
            AnimatorStateInfo targetStateInfo = animator.GetNextAnimatorStateInfo(0);
            if (targetStateInfo.fullPathHash == targetStateHash)
            {
                return targetStateInfo.shortNameHash.ToString();
            }
        }

        return string.Empty;
    }


    private int FindClipIndexByName(string clipName)
    {
        if (clipName != null)
        {
            for (int i = 0; i < animationConfig.ClipName.Count; i++)
            {
                if (animationConfig.ClipName[i] == clipName)
                {
                    return i;
                }
            }
        }
        return -1;
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


    /// <summary>
    /// 获取当前正在播放的动画的AnimatorStateInfo，如果在过渡，就返回过渡目标的动画
    /// </summary>
    /// <returns></returns>
    static public AnimatorStateInfo GetStateInfo(Animator animator)
    {
        AnimatorTransitionInfo transitionInfo = animator.GetAnimatorTransitionInfo(0);

        if (transitionInfo.anyState)
        {
            AnimatorStateInfo targetStateInfo = animator.GetNextAnimatorStateInfo(0);
            return targetStateInfo;
        }
        else
        {
            AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return currentStateInfo;
        }
    }

    /// <summary>
    ///  根据当前的animatorinfo得到动画信息
    /// </summary>
    /// <returns></returns>
    private int GetAnimConfig(AnimatorStateInfo stateInfo, List<string> Names)
    {
        for (int i = 0; i < stateInfo.length; i++)
        {
            if (stateInfo.IsName(Names[i]))
            {
                return i;
            }
        }
        return -1;
    }


}
