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
            //新语法
            GameObject[] gamesEnemy = mainCharacter.enemys.Select(m => m.gameObject).ToArray();

            mainCharacter.selectEnemy = HelpTools01.FindClosest(Attack.CharacterActor.gameObject, gamesEnemy).GetComponent<CharacterInfo>();
            Vector3 Forward = (mainCharacter.selectEnemy.transform.position - Attack.CharacterActor.transform.position).normalized;
            Attack.CharacterActor.Forward = new(Forward.x, 0, Forward.z);
        }
        else
        {
            //没有单位就可以自由转向，但是只能在攻击开始的时候转向
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
            //这样在攻击中的时候，不会被上一次的CannotGetInput重置combo
            Attack.combo = 0;
            Attack.CharacterActor.Animator.SetInteger("combo", 0);
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
                // 执行Hit方法的前提是isOnDetection为true
                // 在这里添加你要执行的代码
                break;
            }
        }
    }



    //_______________________________________________非动画事件分割线_______________________________________________
    private float originalSpeed;
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
    private float originalTimeScale;

    public void FreezeFrames(float freezeDuration)
    {
        Debug.Log(freezeDuration);
        originalTimeScale = Time.timeScale; // 保存原始的时间缩放值
        Time.timeScale = 0f; // 设置时间缩放为0，实现顿帧效果

        // 在指定的时间后恢复时间缩放
        StartCoroutine(ResumeTimeScale(freezeDuration));
    }

    private System.Collections.IEnumerator ResumeTimeScale(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = originalTimeScale; // 恢复原始的时间缩放
    }
}
