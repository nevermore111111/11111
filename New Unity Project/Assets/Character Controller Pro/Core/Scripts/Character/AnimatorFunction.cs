using Cinemachine;

using Lightbug.CharacterControllerPro.Implementation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    //private Action<int> hitActionOfImpulse;
    //private Action<int> hitActionOfPlayFX;



    private void Awake()
    {
        weaponManagers = GetComponentsInChildren<WeaponManager>().ToList();
        Attack = transform.parent.parent.GetComponentInChildren<Attack>();
        CharacterStateController = transform.parent.parent.GetComponentInChildren<CharacterStateController>();
        CameraEffects = CinemachineFreeLook.GetComponent<CameraEffects>();

        //
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
    public void HitStart()
    {
        foreach (var manager in weaponManagers)
        {
            if (manager.isActiveAndEnabled)
            {
                manager.ToggleDetection(true);
                break;
            }
        }
    }
    public void HitReStart()
    {
        foreach (var manager in weaponManagers)
        {
            if (manager.isActiveAndEnabled)
            {
                manager.ToggleDetection(false);
                manager.ToggleDetection(true);
                break;
            }
        }
    }

    public void AttackStart(int num)
    {
        //Attack.combo = num;
        mainCharacter.HitKind =Mathf.Clamp(num,1, 3);
        Attack.isAttack = true;
        Attack.CharacterActor.Animator.SetBool("attack", true);
        Attack.canChangeState = false;
        Attack.OnceAttack = false;
        Attack.CharacterActor.Animator.SetInteger("specialAttack", 0);

        if (mainCharacter.enemies.Count != 0)
        {
            //新语法
            GameObject[] gamesEnemy = mainCharacter.enemies.Select(m => m.gameObject).ToArray();
            mainCharacter.selectEnemy = HelpTools01.FindClosest(Attack.CharacterActor.gameObject, gamesEnemy).GetComponent<CharacterInfo>();
            Vector3 Forward = (mainCharacter.selectEnemy.transform.position - Attack.CharacterActor.transform.position).normalized;
            Attack.CharacterActor.Forward = new(Forward.x, 0, Forward.z);
        }
        else
        {
            //没有单位就可以自由转向，但是只能在攻击开始的时候转向
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
        //        // 逐渐转向目标方向
        //        float rotationSpeed = 200f; // 转身速度（度/秒）
        //        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        //        Quaternion currentRotation = Attack.CharacterActor.transform.rotation;

        //        while (Quaternion.Angle(currentRotation, targetRotation) > 0.1f)
        //        {
        //            currentRotation = Quaternion.RotateTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        //            Attack.CharacterActor.transform.rotation = currentRotation;
        //            Debug.Log("开启");
        //            yield return null; // 等待一帧更新
        //        }
        //    }

        //   // Attack.CharacterActor.Forward = new Vector3(targetDirection.x, 0f, targetDirection.z);
        //}
        //else
        //{
        //    Attack.CharacterActor.Forward = CharacterStateController.InputMovementReference;
        //}
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


    public void Hit(int attackHitRank)
    {
        //foreach (var manager in weaponManagers)
        //{
        //    if (manager.isActiveAndEnabled && manager.isHited == true)
        //    {
           
        //        switch (attackHitRank)
        //        {
                    
        //            case 0:
        //                {
                            
        //                    break;
        //                }
        //            case 1:
        //                {
        //                    //StartCoroutine(AdjustTimeScaleOverDuration(0.03f, 0.05f, 0.06f, 0.2f, manager));
        //                    //hitActionOfImpulse += manager.Impluse;
        //                    //hitActionOfPlayFX += manager.PlayHittedFx;
        //                    break;
        //                }
        //            case 2:
        //                {
        //                    //StartCoroutine(AdjustTimeScaleOverDuration(0.03f, 0.06f, 0.1f, 0.05f, manager));
        //                    break;
        //                }
        //            case 3:
        //                {
        //                    break;
        //                }


        //        }

        //        // manager.Impluse();
             
        //        return;
        //    }
        //}
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



    /// <summary>
    /// 一个时停加震动的复合方法。
    /// </summary>
    /// <param name="fadeInDuration"></param>
    /// <param name="fadeOutDuration"></param>
    /// <param name="duration"></param>
    /// <param name="targetTimeScale"></param>
    /// <param name="weaponManager"></param>
    /// <returns></returns>
    public System.Collections.IEnumerator AdjustTimeScaleOverDuration(float fadeInDuration, float fadeOutDuration, float duration, float targetTimeScale, WeaponManager weaponManager)
    {
        float initialTimeScale = Time.timeScale;
        float elapsedTime = 0f;

        // 渐入
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / fadeInDuration);
            Time.timeScale = Mathf.Lerp(initialTimeScale, targetTimeScale, normalizedTime);
            // 可以在这里根据需要进行其他的逻辑处理

            // 等待一帧
            yield return null;
        }
       
        // 设置目标时间缩放
        Time.timeScale = targetTimeScale;
        weaponManager.PlayHittedFx();
        // 持续时间
        yield return new WaitForSecondsRealtime(duration);

        // 渐出
        elapsedTime = 0f;

        //调用震动和特效
        weaponManager.Impluse();
        //这里需要调用两个地方产生特效，一个是自身的刀光额外特效，另外一个是怪物的受击反馈。
        //需要做个委托


        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / fadeOutDuration);
            Time.timeScale = Mathf.Lerp(targetTimeScale, 1f, normalizedTime);
            // 可以在这里根据需要进行其他的逻辑处理

            // 等待一帧
            yield return null;
        }
        //一般在时停的最后时间再去调用摄像机的震动效果。
        // 恢复原始的时间缩放
        Time.timeScale = 1f;
    }



}
