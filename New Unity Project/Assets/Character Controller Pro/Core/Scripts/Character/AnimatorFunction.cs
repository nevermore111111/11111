using Cinemachine;
using Codice.CM.SEIDInfo;
using DG.DemiEditor;
using JetBrains.Annotations;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<WeaponManager> weaponManagers;


    private void Awake()
    {
        weaponManagers = GetComponentsInChildren<WeaponManager>().ToList();
        Attack =transform.parent.parent.GetComponentInChildren<Attack>();
        CharacterStateController = transform.parent.parent.GetComponentInChildren<CharacterStateController>();
        CameraEffects = CinemachineFreeLook.GetComponent<CameraEffects>();
    }

    public void Idle()
    {
        if (Attack.isJustEnter && (CharacterStateController.PreviousState.GetType() != typeof(Attack)))
        {
            Attack.isJustEnter = false;
            Attack.combo = 1;
            Attack.CharacterActor.Animator.SetInteger("combo", Attack.combo);
            Attack.canChangeState = false;
        }
        else
        {
            Attack.isAttack = false;
            Attack.CharacterActor.Animator.SetBool("attack", false);
            Attack.combo = 0;
            Attack.CharacterActor.Animator.SetInteger("combo", Attack.combo);
            Attack.canInput = true;
            Attack.canChangeState = true;
        }
    }
    public void NormalIdle()
    {
        Attack.CharacterActor.SetUpRootMotion(false, false);
    }
    public void Stop()
    {

        Attack.CharacterActor.Animator.SetBool("stop", false);
    }
    public void AttackEnd()
    {
        if (!Attack.CharacterActor.Animator.IsInTransition(0))
        {
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
    public void AttackStart(int num)
    {
        
        foreach (var manager in weaponManagers)
        {
            if (manager.isActiveAndEnabled)
            {
                manager.ToggleDetection(true);
                break;
            }
        }
        Attack.combo = num;
        Attack.isAttack = true;
        Attack.CharacterActor.Animator.SetBool("attack", true);
        Attack.canChangeState = false;
        Attack.OnceAttack = false;
        Attack.CharacterActor.Animator.SetInteger("specialAttack", 0);
        //if(Input.GetButton("Lock"))
        //{
        //    CharacterActor.Forward = Vector3.zero;
        //}
        if (mainCharacter.enemys.Count != 0)
        {
            //���﷨
            GameObject[] gamesEnemy = mainCharacter.enemys.Select(m => m.gameObject).ToArray();

            mainCharacter.selectEnemy = HelpTools01.FindClosest(Attack.CharacterActor.gameObject, gamesEnemy).GetComponent<CharacterInfo>();
            Vector3 Forward = (mainCharacter.selectEnemy.transform.position - Attack.CharacterActor.transform.position).normalized;
            Attack.CharacterActor.Forward = new(Forward.x, 0, Forward.z);
        }
        else
        {
            //û�е�λ�Ϳ�������ת�򣬵���ֻ���ڹ�����ʼ��ʱ��ת��
            Attack.CharacterActor.Forward = CharacterStateController.InputMovementReference;
        }
    }
    public void CanGetInput()
    {
        Attack.canInput = true;
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
       Attack. CharacterActor.Animator.SetTrigger("end");
    }
    public void Hit(int attackHitRank)
    {
        foreach (var manager in weaponManagers)
        {
            if (manager.isActiveAndEnabled&&manager.isHited ==true)
            {
                CameraEffects.ShakeCamera();
                //SlowDownAnimator(0.01f, 0.15f);
                FreezeFrames(0.1f);
                // ִ��Hit������ǰ����isOnDetectionΪtrue
                // �����������Ҫִ�еĴ���
                break;
            }
        }
    }



    //_______________________________________________�Ƕ����¼��ָ���_______________________________________________
    private float originalSpeed;
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
    private float originalTimeScale;

    public void FreezeFrames(float freezeDuration)
    {
        Debug.Log(freezeDuration);
        originalTimeScale = Time.timeScale; // ����ԭʼ��ʱ������ֵ
        Time.timeScale = 0f; // ����ʱ������Ϊ0��ʵ�ֶ�֡Ч��

        // ��ָ����ʱ���ָ�ʱ������
        StartCoroutine(ResumeTimeScale(freezeDuration));
    }

    private System.Collections.IEnumerator ResumeTimeScale(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = originalTimeScale; // �ָ�ԭʼ��ʱ������
    }
}
