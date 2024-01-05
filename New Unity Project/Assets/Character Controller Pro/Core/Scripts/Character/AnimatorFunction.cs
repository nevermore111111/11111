using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Implementation;
using MagicaCloth2;
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
    private WeaponManager weaponManager;
    private Attack Attack;
    CharacterStateController CharacterStateController;
    public CinemachineFreeLook CinemachineFreeLook;
    CameraEffects CameraEffects;
    private List<WeaponManager> weaponManagers;//�������������
    private AnimationConfig animationConfig;//����Ƕ�������
    public SoloAnimaConfig CurrentAnimConfig;//�����������¼����ʱ��Ĳ���
    public int currentHitIndex;//��ǰ��������ĵ�n�ι������
    private int currentAnimPar;
    public int hitKind;//���ڹ���������
    public string activeWeaponDetect;//���ڼ������ײ����
    public string currentStateName;//��ǰ���ڲ��ŵĶ���
    public WeaponData WeaponData;
    CharacterActor characterActor;

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
        characterActor = GetComponentInParent<CharacterActor>();
        //
    }
    private void Start()
    {
        animationConfig = DataLoad.animationConfig;
    }
    public void JumpStart()
    {
        //�رն��������������
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
        characterActor.Animator.SetBool(Attack.stopParameter, false);
    }

    public void AttackEnd()
    {
        characterActor.Animator.speed = 1f;
        if (!characterActor.Animator.IsInTransition(0))
        {
            if (CharacterStateController.CurrentState is Attack)
                characterActor.SetUpRootMotion(true, true);
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
        Debug.LogError("��ʼ����");
        characterActor.Animator.speed = 1.3f;
        SetStrengthAndDetector();

        //���ݵ�ǰ�������������
        //���ݵ�ǰ��detections���е�����������detection;

        SetWeaponDirection();
        foreach (var manager in weaponManagers)
        {
            if (manager.isActiveAndEnabled)
            {
                manager.ToggleDetection(true);
                if (manager != null)
                {
                    //���ݶ������������Ӧ����ײ����
                    manager.weaponFx = CurrentAnimConfig.HittedEffect;
                    ActiveDetectionByStringPar(activeWeaponDetect, manager);

                }
                break;
            }
        }
        currentHitIndex++;
    }

    /// <summary>
    /// �Ƿ�Ҫ�Զ�������������������Զ�����ȥ����attack��ʼʱһ��ȥ���Զ�
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
        //������ǰ�Ĺ�������
        if (CurrentAnimConfig.AttackDirection.Length < 3)
        {
            //�����û��д
            //Ҫ��������
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
            //����ݵ�ǰ�˴ι����Ĵ���ȥ����Ӧ�Ĺ���
            foreach (WeaponManager manager in weaponManagers)
            {
                if (manager.isActiveAndEnabled)
                {
                    weaponManager = manager;
                    manager.isNeedUpdateDirection = false;
                    Vector3 DirectionIncharacter = new Vector3(CurrentAnimConfig.AttackDirection[currentHitIndex * 3], CurrentAnimConfig.AttackDirection[currentHitIndex * 3 + 1], CurrentAnimConfig.AttackDirection[currentHitIndex * 3 + 2]);
                    //Ȼ������Ҫ�����
                    manager.WeaponDirection = Attack.transform.TransformDirection(DirectionIncharacter);
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
        //���￪ʼsp���������ݵ�ǰsp���������ƣ����õ�ǰspattack��Ϣ
        Attack.SpAttack = Mathf.RoundToInt(CurrentAnimConfig.SpAttackPar[0]);
        //���ݵ�ǰ�Ķ�����������
    }

    /// <summary>
    /// ������ʼ
    /// </summary>
    /// <param name="attackName"></param>
    public void AttackStart(string attackName)
    {
        //������ǰ�Ĺ�������

        characterActor.Animator.speed = 1f;
        characterActor.UseRootMotion = true;
        //���¹������ƺ����ñ�
        ResetCurrentInfo(attackName);

        //��ʼһ�ι���
        Attack.isNextAttack = false;//��������Ѿ�ִ������һ�ι���
        Attack.isAttack = true;
        characterActor.Animator.SetBool("attack", true);
        Attack.canChangeState = false;
        Attack.OnceAttack = false;
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
        if(characterActor.CharacterInfo.selectEnemy != null)
        {
            CharacterActor enemyActor = characterActor.CharacterInfo.selectEnemy.characterActor;
            DOTween.To(enemyActor.Position, () => { },skillReceiver.transform.position,)
        }
    }

    public void Drop()
    {
        characterActor.UseRootMotion = false;
        Attack.useGravity = true;
        characterActor.VerticalVelocity -= 10f * characterActor.Up;
        characterActor.alwaysNotGrounded = false;
        //����ʹ��ˮƽ�ƶ�
    }
    public void CannotGetInput()
    {
        if (!Attack.isAttack)
        {
            //�����ڹ����е�ʱ�򣬲��ᱻ��һ�ε�CannotGetInput����combo
            Attack.combo = 0;
            characterActor.Animator.SetInteger("combo", 0);
        }
    }
    /// <summary>
    /// ��ҿ��Կ���
    /// </summary>
    public void CanPlayerControl()
    {
        //���֮ǰ���ǣ���ô�Ϳ�ʼ������
        if (CharacterStateController.PreviousState is StartPlay)
        {
            CharacterState.canPlayerControl = true;
        }
    }





    /// <summary>
    /// ��һ�����п�ʼʱִ�У�����������е��������������
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




    //_______________________________________________�Ƕ����¼��ָ���_______________________________________________
    //_______________________________________________�Ƕ����¼��ָ���_______________________________________________
    //_______________________________________________�Ƕ����¼��ָ���_______________________________________________


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
    ///���ݶ���ʱ�䴫�ݵĲ�����ȷ�ϵ�ǰӦ�ü���ļ������
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
    /// ���û�е��˾������������еĻ���ת����ˣ����һ�������������˵ľ�������Ƿ���rootmotion
    /// </summary>
    private void ResetAttackRootAndrotate()
    {

        if (mainCharacter.enemies.Count != 0)
        {
            //���﷨
            GameObject[] gamesEnemy = mainCharacter.enemies.Select(m => m.gameObject).ToArray();
            SetActorForword(gamesEnemy);
            //Debug.Log(Attack.CharacterActor.Forward);
            if ((transform.position - mainCharacter.selectEnemy.transform.position).magnitude < 1.5f)
            {

                if (Attack.SpAttack == -1)
                {
                    characterActor.SetUpRootMotion(false, false);
                }
                characterActor.PlanarVelocity = Vector3.zero;
            }
        }
        else
        {
            //û�е�λ�Ϳ�������ת�򣬵���ֻ���ڹ�����ʼ��ʱ��ת��
            characterActor.Forward = CharacterStateController.InputMovementReference;
        }
    }

    /// <summary>
    /// ��������������
    /// </summary>
    /// <param name="gamesEnemy"></param>
    private void SetActorForword(GameObject[] gamesEnemy)
    {


        mainCharacter.selectEnemy = HelpTools01.FindClosest(characterActor.gameObject, gamesEnemy).GetComponent<CharacterInfo>();
        Vector3 Forward = (mainCharacter.selectEnemy.transform.position - characterActor.transform.position).normalized;
        //��ǰ����Ŀ�ֱ꣬���Զ�ת
        if (Vector3.Angle(characterActor.Forward, Forward) < Attack.maxAutoAnglerotate)
        {
            characterActor.Forward = new(Forward.x, 0, Forward.z);
            characterActor.Up = Vector3.up;
        }
        else //��ǰû�����򹥻�Ŀ����Ҫ������Ŀ��ķ���ת
        {
            Vector3 target = CharacterStateController.InputMovementReference;
            if (target != Vector3.zero)
            {
                target.z = 0;
                target.Normalize();
                //������Ҫת���ĽǶ�
                float angle = Vector3.Angle(target, characterActor.Forward) > Attack.maxAttackAngleNoenemy ? Attack.maxAttackAngleNoenemy : Vector3.Angle(target, characterActor.Forward);
                characterActor.Forward = RotateVectorAroundAxis(characterActor.Forward, target, angle);
                characterActor.Forward = Vector3.ProjectOnPlane(characterActor.Forward, Vector3.up);
                characterActor.Up = Vector3.up;
            }
        }

        Vector3 RotateVectorAroundAxis(Vector3 vector, Vector3 axis, float angle)
        {
            // ����������ת������Ԫ��
            Quaternion rotation = Quaternion.AngleAxis(angle, axis);

            // ������������ת
            Vector3 rotatedVector = rotation * vector;

            return rotatedVector;
        }
    }

    /// <summary>
    /// ���¸��ݶ������Ƹ���������Ϣ
    /// </summary>
    /// <param name="attackName"></param>
    private void ResetCurrentInfo(string attackName)
    {
        //�������һ�²����κ�����
        if (currentStateName == attackName)
        {
            currentStateName = attackName;
            GetAnimationPar(currentStateName);//���ݵ�ǰ�Ķ��������stateȥ�ö�������
            timelineManager.PlayTimelineByName(CurrentAnimConfig.ClipName); // ���Ŷ�Ӧ���Ƶ�Playable
        }
        else
        {
            currentStateName = attackName;
            GetAnimationPar(currentStateName);//���ݵ�ǰ�Ķ��������stateȥ�ö�������
            timelineManager.PlayTimelineByName(CurrentAnimConfig.ClipName); // ���Ŷ�Ӧ���Ƶ�Playable
        }
        currentHitIndex = 0;
    }


    private void GetAnimationPar(string currentStateName)
    {

        int index = FindStateIndexByName(currentStateName);//���ݵ�ǰ�Ķ����������޸�
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
            Debug.LogError($"û�ҵ�{currentStateName}");
        }
    }


    public void SlowDownAnimator(float slowDownFactor, float duration)
    {
        originalSpeed = characterActor.Animator.speed; // ����ԭʼ�Ĳ����ٶ�
        Debug.Log(originalSpeed);

        characterActor.Animator.speed = originalSpeed * slowDownFactor; // �޸Ĳ����ٶ�Ϊ��ǰ�� slowDownFactor

        // ��ָ����ʱ���ָ�ԭʼ�ٶ�
        StartCoroutine(RestoreAnimatorSpeed(duration));
    }
    private System.Collections.IEnumerator RestoreAnimatorSpeed(float delay)
    {
        yield return new WaitForSeconds(delay);

        characterActor.Animator.speed = originalSpeed; // �ָ�ԭʼ�����ٶ�
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

        if (transitionInfo.anyState)//����ڹ���
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
    /// ��ȡ��ǰ���ڲ��ŵĶ�����AnimatorStateInfo������ڹ��ɣ��ͷ��ع���Ŀ��Ķ���
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
    ///  ���ݵ�ǰ��animatorinfo�õ�������Ϣ
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
