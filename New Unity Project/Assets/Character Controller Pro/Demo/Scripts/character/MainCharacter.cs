using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : CharacterInfo
{
    public CharacterActor CharacterActor;
    public CharacterStateController CharacterStateController;
    public Hitted CharacterHitted;
    public WeaponKind WeaponKind;
    
    public override void GetDamage(float damage, Vector3 pos, WeaponManager weapon, Collider collider,IAgent.HitKind hit = IAgent.HitKind.ground)
    {
        //需要找到主角调用
        CharacterHitted.GetHitted( weapon,hit );
    }


    protected override void Awake()
    {
        base.Awake();
        CharacterActor = GetComponentInParent<CharacterActor>();
        CharacterStateController = this.transform.parent.GetComponentInChildren<CharacterStateController>();
        CharacterHitted = GetComponentInChildren<Hitted>();
    }

#warning(这里没做,摄像机使用的)
    internal bool GetIsAttacked()
    {
        return false;
       
    }

    internal bool GetIsAttacking()
    {
        if(CharacterStateController.CurrentState is Attack)
        {
            return  Attack.isAttack;
        }
        return false;
    }



    internal bool ismoving()
    {
        if (CharacterStateController.CurrentState is NormalMovement)
        {
            NormalMovement normalMovementState = CharacterStateController.CurrentState as NormalMovement;

            // 访问 NormalMovement 实例的变量
            if (normalMovementState != null)
            {

                return normalMovementState.moving;
                // 使用 normalMovementState 访问实例的变量
                // 例如: normalMovementState.SomeVariable
            }
        }
        return false;
    }
    override public void HitOther(WeaponManager weapon)
    {
        //这个需要一个动画时间方法，动画中去更新这次攻击的参数
        //调用击中效果
        HitParByHitKind(weapon);
    }
    
    //这个是我打到别人的方法
    public void HitParByHitKind(WeaponManager weapon)
    {
        switch (HitKind)
        {
            
            case 0:
                {
                    StartCoroutine(Hit(0.03f, 0.03f, 0.03f, 0.3f, weapon));
                    break;
                }
            case 1:
                {
                    StartCoroutine(Hit(0.05f, 0.05f, 0.06f, 0.1f, weapon));
                    break;
                }
            case 2:
                {
                    StartCoroutine(Hit(0.05f, 0.06f, 0.08f, 0.1f, weapon));
                    break;
                }
            case 3:
                {
                    StartCoroutine(Hit(0.05f, 0.05f, 0.08f, 0.1f, weapon));
                    break;
                }
        }
       
    }

    public IEnumerator Hit(float fadeInDuration, float fadeOutDuration, float duration, float targetTimeScale, WeaponManager weaponManager)
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
        if(weaponManager.isActiveAndEnabled)
        {
            weaponManager.PlayHittedFx();
        }
        // 持续时间
        yield return new WaitForSecondsRealtime(duration);

        // 渐出
        elapsedTime = 0f;
        if (weaponManager.isActiveAndEnabled)
        {
      
            //调用震动和特效
            weaponManager.Impluse();
        }
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
