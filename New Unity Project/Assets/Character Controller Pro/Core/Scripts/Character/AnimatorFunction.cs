using Cinemachine;
using Cinemachine.Utility;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Implementation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 
/// <summary>
public class AnimatorFunction : MonoBehaviour
{
    [SerializeField]
    private MainCharacter mainCharacter;
    private WeaponManager weaponManager;//
    private Attack Attack;
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
        Attack = transform.parent.parent.GetComponentInChildren<Attack>();
        CharacterStateController = transform.parent.parent.GetComponentInChildren<CharacterStateController>();
        timelineManager = GetComponent<TimelineManager>();
        WeaponData = FindAnyObjectByType<WeaponData>();
        characterActor = GetComponentInParent<CharacterActor>();
    }
    private void Start()
    {
        animationConfig = DataLoad.Instance.animationConfig;
    }
    public void JumpStart()
    {
        //关闭动画机进入的条件
        characterActor.Animator.SetBool("jump", false);
    }

    public void Idle()
    {
        Attack.isAttack = false;
        characterActor.Animator.SetBool("attack", false);
        Attack.combo = 0;
        characterActor.Animator.SetInteger("combo", Attack.combo);
        Attack.canInput = true;
        Attack.canChangeState = true;
    }

    public void Fly()
    {
        characterActor.alwaysNotGrounded = true;
    }
    public void FlyEnd()
    {
        characterActor.alwaysNotGrounded = false;
    }


    public void NormalIdle()
    {
        if (characterActor.Animator.IsInTransition(0) == false)
            characterActor.SetUpRootMotion(false, false);
    }
    public void Stop()
    {
        //characterActor.Animator.SetBool(Attack.stopParameter, false);
    }

    public void AttackEnd()
    {
        characterActor.Animator.speed = 1f;
        if (!characterActor.Animator.IsInTransition(0))
        {
            if (CharacterStateController.CurrentState is Attack && !DonotUseRootmovtion())
            {
                characterActor.SetUpRootMotion(true, true);
            }
            Attack.isAttack = false;
            characterActor.Animator.SetBool("attack", false);
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
        characterActor.Animator.speed = 1f;
        if (!characterActor.Animator.IsInTransition(0))
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



    public void HitStart()//int hitKind, string activeWeaponDetect
    {
        Debug.LogError("开始攻击");
        //characterActor.Animator.speed = 1.1f;
        SetStrengthAndDetector();

        //根据当前攻击类别来进行
        //根据当前的detections进行调整这个激活的detection;

        SetWeaponDirection();
        foreach (var manager in weaponManagers)
        {
            if (manager.isActiveAndEnabled)
            {
                manager.ToggleDetection(true);
                if (manager != null)
                {
                    //根据动画参数激活对应的碰撞区域
                    manager.weaponHitFx = CurrentAnimConfig.HittedEffect;
                    ActiveDetectionByStringPar(activeWeaponDetect, manager);

                }
                break;
            }
        }
        currentHitIndex++;
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
                    Vector3 DirectionIncharacter = new Vector3(CurrentAnimConfig.AttackDirection[currentHitIndex * 3], CurrentAnimConfig.AttackDirection[currentHitIndex * 3 + 1], CurrentAnimConfig.AttackDirection[currentHitIndex * 3 + 2]);
                    //然后我需要把这个
                    manager.WeaponWorldDirection = Attack.transform.TransformDirection(DirectionIncharacter);//这个时候已经转化成世界方向了
                    break;
                }
            }
        }
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

        characterActor.Animator.speed = 1f;
        characterActor.UseRootMotion = true;
        //更新攻击名称和配置表
        ResetCurrentInfo(attackName);

        //开始一次攻击
        Attack.isNextAttack = false;//这个代表已经执行了下一次攻击
        Attack.isAttack = true;
        characterActor.Animator.SetBool("attack", true);
        Attack.canChangeState = false;
        characterActor.Animator.SetInteger("specialAttack", 0);
        ResetAttackRootAndrotate();
    }


    public void PlayTimeline(string TimelineName)
    {
        timelineManager.PlayTimelineByName(TimelineName);
    }

    public void CanGetInput()
    {
        characterActor.Animator.SetInteger("specialAttack", 0);
        Attack.canInput = true;
        Attack.SpAttack = -1;
    }

    public void LetSelectEnemyCloser(int requireSkillNum)
    {
        SkillReceiver skillReceiver = characterActor.CharacterInfo.GetSkillReceiver(requireSkillNum);
        if (characterActor.CharacterInfo.selectEnemy != null)
        {
            CharacterActor enemyActor = characterActor.CharacterInfo.selectEnemy.characterActor;
            DOTween.To(() => enemyActor.Position, (Value) => { enemyActor.Position = Value; }, skillReceiver.transform.position, 0.1f);
        }
    }

    public void Drop()
    {
        characterActor.UseRootMotion = false;
        Attack.useGravity = true;
        characterActor.VerticalVelocity -= 10f * characterActor.Up;
        characterActor.alwaysNotGrounded = false;
        //仅仅使用水平移动
    }
    public void CannotGetInput()
    {
        if (!Attack.isAttack)
        {
            //这样在攻击中的时候，不会被上一次的CannotGetInput重置combo
            Attack.combo = 0;
            characterActor.Animator.SetInteger("combo", 0);
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
        characterActor.Animator.SetInteger("specialAttack", 0);
    }
    public void SpAtk(int kind)
    {
        characterActor.Animator.SetInteger("specialAttack", kind);
    }
    public void End()
    {
        characterActor.Animator.SetTrigger("end");
    }




    //_______________________________________________非动画事件分割线_______________________________________________
    //_______________________________________________非动画事件分割线_______________________________________________
    //_______________________________________________非动画事件分割线_______________________________________________


    private float originalSpeed;


    private void SetStrengthAndDetector()
    {
        if (CurrentAnimConfig.HitStrength.Length > currentHitIndex)
            hitKind = CurrentAnimConfig.HitStrength[currentHitIndex];
        if (CurrentAnimConfig.HitDetect.Length > currentHitIndex)
            activeWeaponDetect = CurrentAnimConfig.HitDetect[currentHitIndex];

        mainCharacter.HitStrength = hitKind;
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

        if (mainCharacter.enemies.Count != 0)
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
        //距离很近，并且非特殊攻击
        if (mainCharacter.selectEnemy != null)
            return ((Attack.SpAttack == -1) && (transform.position - mainCharacter.selectEnemy.transform.position).magnitude < Attack.CharacterAttackDistance);
        else
            return (Attack.SpAttack == -1);

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
            mainCharacter.selectEnemy = characterActor.CharacterInfo.enemies.OrderBy((_) => (_.transform.position - mainCharacter.transform.position).sqrMagnitude).First();
            needAutoRotate = true;
        }
        else if (CharacterStateController.InputMovementReference != Vector3.zero)
        {
            //否则去按输入转向，定义转向速度和时间。
            Vector3 target = CharacterStateController.InputMovementReference.ProjectOntoPlane(Vector3.up).normalized;//我想转向的方向
            Quaternion  targetQua = Quaternion.LookRotation(target, Vector3.up);
            DOTween.To(()=>0f, value => 
            {
                //旋转一定角度
                Quaternion.RotateTowards(characterActor.transform.rotation,targetQua, 0.1f*Time.deltaTime);
            }, 1f, 0.1f);
            //DOTween.To();//执行

        }
        //选择的敌人不为0，自动去选择攻击单位
        if (needAutoRotate)
        {
            //我应该面向的方向
            Vector3 targetMainCharacterForward = (mainCharacter.selectEnemy.transform.position - mainCharacter.transform.position).ProjectOntoPlane(Vector3.up).normalized;
            characterActor.transform.rotation = Quaternion.LookRotation(targetMainCharacterForward, Vector3.up);
        }

        //mainCharacter.selectEnemy = HelpTools01.FindClosest(characterActor.gameObject, gamesEnemy).GetComponent<CharacterInfo>();
        //Vector3 Forward = (mainCharacter.selectEnemy.transform.position - characterActor.transform.position).normalized;
        ////当前面向目标，直接自动转
        //if (Vector3.Angle(characterActor.Forward, Forward) < Attack.maxAutoAnglerotate)
        //{
        //    characterActor.Forward = new(Forward.x, 0, Forward.z);
        //    characterActor.Up = Vector3.up;
        //}
        //else //当前没有面向攻击目标需要向输入目标的方向转
        //{
        //Vector3 target = CharacterStateController.InputMovementReference;
        //    if (target != Vector3.zero)
        //    {
        //        target.z = 0;
        //        target.Normalize();
        //        //计算需要转动的角度
        //        float angle = Vector3.Angle(target, characterActor.Forward) > Attack.maxAttackAngleNoenemy ? Attack.maxAttackAngleNoenemy : Vector3.Angle(target, characterActor.Forward);
        //        characterActor.Forward = RotateVectorAroundAxis(characterActor.Forward, target, angle);
        //        characterActor.Forward = Vector3.ProjectOnPlane(characterActor.Forward, Vector3.up);
        //        characterActor.Up = Vector3.up;
        //    }
        //}

        //Vector3 RotateVectorAroundAxis(Vector3 vector, Vector3 axis, float angle)
        //{
        //    // 将向量和轴转换成四元数
        //    Quaternion rotation = Quaternion.AngleAxis(angle, axis);

        //    // 将向量绕轴旋转
        //    Vector3 rotatedVector = rotation * vector;

        //    return rotatedVector;
        //}
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


    public void SlowDownAnimator(float slowDownFactor, float duration)
    {
        originalSpeed = characterActor.Animator.speed; // 保存原始的播放速度
        Debug.Log(originalSpeed);

        characterActor.Animator.speed = originalSpeed * slowDownFactor; // 修改播放速度为当前的 slowDownFactor

        // 在指定的时间后恢复原始速度
        StartCoroutine(RestoreAnimatorSpeed(duration));
    }
    private System.Collections.IEnumerator RestoreAnimatorSpeed(float delay)
    {
        yield return new WaitForSeconds(delay);

        characterActor.Animator.speed = originalSpeed; // 恢复原始播放速度
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
