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
    WeaponManager WeaponManager;
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

    private TimelineManager timelineManager;
    //private Action<int> hitActionOfImpulse;
    //private Action<int> hitActionOfPlayFX;



    private void Awake()
    {
        weaponManagers = GetComponentsInChildren<WeaponManager>().ToList();
        Attack = transform.parent.parent.GetComponentInChildren<Attack>();
        CharacterStateController = transform.parent.parent.GetComponentInChildren<CharacterStateController>();
        CameraEffects = CinemachineFreeLook.GetComponent<CameraEffects>();
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
        //�رն��������������
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
        if(Attack.CharacterActor.Animator.IsInTransition(0)==false)
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
            if(CharacterStateController.CurrentState is Attack)
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
        
        hitKind = CurrentAnimConfig.HitStrength[currentHitIndex];
        activeWeaponDetect = CurrentAnimConfig.HitDetect[currentHitIndex];
        //���õ�ǰ�������
        mainCharacter.HitStrength = hitKind;
        //���ݵ�ǰ�������������
        //���ݵ�ǰ��detections���е�����������detection;
        currentHitIndex++;
        foreach (var manager in weaponManagers)
        {
            if (manager.isActiveAndEnabled)
            {
                manager.ToggleDetection(true);
                if (manager != null)
                {
                    //���ݶ������������Ӧ����ײ����
                    ActiveDetectionByStringPar(activeWeaponDetect, manager);

                    switch (hitKind)
                    {
                        case 0:
                            manager.AdjustFrequencyAndAmplitude(1, 0.5f);
                            break;
                        case 1:
                            manager.AdjustFrequencyAndAmplitude(1.5f, 0.4f);
                            break;
                        case 2:
                            manager.AdjustFrequencyAndAmplitude(2f, 0.4f);
                            break;
                        case 4:
                            manager.AdjustFrequencyAndAmplitude(3f, 0.4f);
                            break;
                        default:
                            manager.AdjustFrequencyAndAmplitude(1f, 1f);
                            break;
                    }
                }
                break;
            }
        }
    }
    public void HitReStart()//int Hit = 1, string activeWeaponDetect = null
    {
        currentHitIndex++;
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
    ///���ݶ���ʱ�䴫�ݵĲ�����ȷ�ϵ�ǰӦ�ü���ļ������
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
        Attack.CharacterActor.Animator.speed = 1.2f;
        Attack.CharacterActor.UseRootMotion = true;
        //�������һ�²����κ�����
        if( currentStateName ==attackName)
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


        Attack.isNextAttack = false;//��������Ѿ�ִ������һ�ι���
        Attack.isAttack = true;
        Attack.CharacterActor.Animator.SetBool("attack", true);
        Attack.canChangeState = false;
        Attack.OnceAttack = false;
        Attack.CharacterActor.Animator.SetInteger("specialAttack", 0);

        if (mainCharacter.enemies.Count != 0)
        {
            //���﷨
            GameObject[] gamesEnemy = mainCharacter.enemies.Select(m => m.gameObject).ToArray();
            mainCharacter.selectEnemy = HelpTools01.FindClosest(Attack.CharacterActor.gameObject, gamesEnemy).GetComponent<CharacterInfo>();
            Vector3 Forward = (mainCharacter.selectEnemy.transform.position - Attack.CharacterActor.transform.position).normalized;
            //Debug.Log(Attack.CharacterActor.Forward);
            Attack.CharacterActor.Forward = new(Forward.x, 0, Forward.z);
            //Debug.Log(Attack.CharacterActor.Forward);
            if ((transform.position - mainCharacter.selectEnemy.transform.position).magnitude < 1.5f)
            {
                //Attack.CharacterActor.PlanarVelocity = Vector3.zero;
                //if (Attack.SpAttack == -1)
                //{
                //    Attack.CharacterActor.SetUpRootMotion(false, false);
                //}
            }
        }
        else
        {
            //û�е�λ�Ϳ�������ת�򣬵���ֻ���ڹ�����ʼ��ʱ��ת��
            Attack.CharacterActor.Forward = CharacterStateController.InputMovementReference;
        }
        //if (mainCharacter.enemys.Count != 0)
        //{
        //    GameObject[] gamesEnemy = mainCharacter.enemys.Select(m => m.gameObject).ToArray();
        //    mainCharacter.selectEnemy = HelpTools01.FindClosest(Attack.CharacterActor.gameObject, gamesEnemy).GetComponent<CharacterInfo>();
        //    Vector3 targetDirection = (mainCharacter.selectEnemy.transform.position - Attack.CharacterActor.transform.position).normalized;

        //    float angleDifference = Vector3.Angle(targetDirection, Attack.CharacterActor.Forward);
        //    if (angleDifference <= 60f)
        //    {
        //        // ��ת��Ŀ�귽��
        //        float rotationSpeed = 200f; // ת���ٶȣ���/�룩
        //        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        //        Quaternion currentRotation = Attack.CharacterActor.transform.rotation;

        //        while (Quaternion.Angle(currentRotation, targetRotation) > 0.1f)
        //        {
        //            currentRotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        //            Attack.CharacterActor.transform.rotation = currentRotation;
        //            Debug.Log("����");
        //            yield return null; // �ȴ�һ֡����
        //        }
        //    }

        //   // Attack.CharacterActor.Forward = new Vector3(targetDirection.x, 0f, targetDirection.z);
        //}
        //else
        //{
        //    Attack.CharacterActor.Forward = CharacterStateController.InputMovementReference;
        //}
    }

    public void PlayTimeline(string TimelineName)
    {
        timelineManager.PlayTimelineByName(TimelineName);
    }

    public void CanGetInput()
    {

        Attack.CharacterActor.Animator.SetInteger("specialAttack",0);
        Attack.canInput = true;
        Attack.SpAttack = -1;
    }
    
    public void Drop()
    {
        Attack.CharacterActor.UseRootMotion = false;
        Attack.useGravity = true;
        Attack.CharacterActor.VerticalVelocity -= WeaponData.DropSpeed*Attack.CharacterActor.Up;
        Attack.CharacterActor.alwaysNotGrounded = false;
        //����ʹ��ˮƽ�ƶ�
    }
    public void CannotGetInput()
    {
        if (!Attack.isAttack)
        {
            //�����ڹ����е�ʱ�򣬲��ᱻ��һ�ε�CannotGetInput����combo
            Attack.combo = 0;
            Attack.CharacterActor.Animator.SetInteger("combo", 0);
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






    //_______________________________________________�Ƕ����¼��ָ���_______________________________________________
    private float originalSpeed;

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
                animationConfig.SpAttackPar[index]
            );
        }
        else
        {
            Debug.LogError($"û�ҵ�{currentStateName}");
        }
    }


    public void SlowDownAnimator(float slowDownFactor, float duration)
    {
        originalSpeed = Attack.CharacterActor.Animator.speed; // ����ԭʼ�Ĳ����ٶ�
        Debug.Log(originalSpeed);

        Attack.CharacterActor.Animator.speed = originalSpeed * slowDownFactor; // �޸Ĳ����ٶ�Ϊ��ǰ�� slowDownFactor

        // ��ָ����ʱ���ָ�ԭʼ�ٶ�
        StartCoroutine(RestoreAnimatorSpeed(duration));
    }
    private System.Collections.IEnumerator RestoreAnimatorSpeed(float delay)
    {
        yield return new WaitForSeconds(delay);

        Attack.CharacterActor.Animator.speed = originalSpeed; // �ָ�ԭʼ�����ٶ�
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
    private int GetAnimConfig(AnimatorStateInfo stateInfo,List<string> Names)
    {
        for(int i = 0;i<stateInfo.length;i++)
        {
            if( stateInfo.IsName(Names[i]))
            {
                return i;
            }
        }
        return -1;
    }


}
