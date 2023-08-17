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
    public void HitStart(int hitKind,string activeWeaponDetect)
    {
        //设置当前攻击类别
        mainCharacter.HitKind = hitKind;
        //根据当前攻击类别来进行
        //
        //根据当前的detections进行调整这个激活的detection;

        foreach (var manager in weaponManagers)
        {
            if (manager.isActiveAndEnabled)
            {
                manager.ToggleDetection(true);
                if (manager != null)
                {
                    //根据动画参数激活对应的碰撞区域
                    int[] weaponIndexes = ConvertStringToIntArray(activeWeaponDetect);
                    manager.ActiveWeaponDetectors = weaponIndexes.Select(index => (WeaponDetector)index).ToArray();

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
    public void HitReStart(int Hit = 1)
    {
        mainCharacter.HitKind = Hit;
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
            if ((transform.position - mainCharacter.selectEnemy.transform.position).magnitude < 1.5f)
            {
                Attack.CharacterActor.PlanarVelocity = Vector3.zero;
                Attack.CharacterActor.SetUpRootMotion(false, false);
            }
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
    //public System.Collections.IEnumerator AdjustTimeScaleOverDuration(float fadeInDuration, float fadeOutDuration, float duration, float targetTimeScale, WeaponManager weaponManager)
    //{
    //    float initialTimeScale = Time.timeScale;
    //    float elapsedTime = 0f;

    //    // 渐入
    //    while (elapsedTime < fadeInDuration)
    //    {
    //        elapsedTime += Time.unscaledDeltaTime;
    //        float normalizedTime = Mathf.Clamp01(elapsedTime / fadeInDuration);
    //        Time.timeScale = Mathf.Lerp(initialTimeScale, targetTimeScale, normalizedTime);
    //        // 可以在这里根据需要进行其他的逻辑处理

    //        // 等待一帧
    //        yield return null;
    //    }

    //    // 设置目标时间缩放
    //    Time.timeScale = targetTimeScale;
    //    weaponManager.PlayHittedFx();
    //    // 持续时间
    //    yield return new WaitForSecondsRealtime(duration);

    //    // 渐出
    //    elapsedTime = 0f;

    //    //调用震动和特效
    //    weaponManager.Impluse();
    //    //这里需要调用两个地方产生特效，一个是自身的刀光额外特效，另外一个是怪物的受击反馈。
    //    //需要做个委托


    //    while (elapsedTime < fadeOutDuration)
    //    {
    //        elapsedTime += Time.unscaledDeltaTime;
    //        float normalizedTime = Mathf.Clamp01(elapsedTime / fadeOutDuration);
    //        Time.timeScale = Mathf.Lerp(targetTimeScale, 1f, normalizedTime);
    //        // 可以在这里根据需要进行其他的逻辑处理

    //        // 等待一帧
    //        yield return null;
    //    }
    //    //一般在时停的最后时间再去调用摄像机的震动效果。
    //    // 恢复原始的时间缩放
    //    Time.timeScale = 1f;
    //}



}
