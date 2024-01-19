using Cysharp.Threading.Tasks;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : CharacterInfo
{
    public CharacterStateController CharacterStateController;
    public Hitted CharacterHitted;
    public WeaponKind WeaponKind;
    public HitData hitData;

    public override void GetDamage(float damage, Vector3 pos, WeaponManager weapon, Collider collider, IAgent.HitKind hit = IAgent.HitKind.ground)
    {
        //需要找到主角调用
        CharacterHitted.GetHitted(weapon, hit);
        if (CharacterStateController.CurrentState is NormalMovement) 
        {

        }

    }


    protected override void Awake()
    {
        base.Awake();
        CharacterStateController = this.transform.parent.GetComponentInChildren<CharacterStateController>();
        CharacterHitted = characterActor.GetComponentInChildren<Hitted>();
        hitData = FindObjectOfType<HitData>();
    }

#warning(这里没做,摄像机使用的)
    internal bool GetIsAttacked()
    {
        return false;

    }

    internal bool GetIsAttacking()
    {
        if (CharacterStateController.CurrentState is Attack)
        {
            return Attack.isAttack;
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

#if UNITY_EDITOR
        // 只在Unity编辑器中运行的代码
        if (hitData.ForceCurrentHit != -1)
        {
            //如果强制使用这个震动，会播放这个震动的震屏效果
            HitStrength = hitData.ForceCurrentHit;
        }
#endif
        HitPlus(HitStrength, hitData, weapon);
    }
    public void HitPlus(int currentHit, HitData hitData, WeaponManager weaponManager)
    {
        float fadeInDuration = hitData.GetFadeTime(hitData, currentHit);
        float fadeOutDuration = hitData.GetFadeTime(hitData, currentHit); ; // 如果渐出时间和渐入时间相同
        float duration = hitData.GetStayTime(hitData, currentHit);
        float targetTimeScale = hitData.GetTimeScale(hitData, currentHit);
        TimeScaleManager.Instance.SetTimeScale(fadeInDuration, fadeOutDuration, duration, targetTimeScale);
        if (weaponManager.isActiveAndEnabled)
        {
            //调用震动和特效
            weaponManager.Impluse();
        }
    }

    public override void GetDamage(float damage, Vector3 attackDirection, float hitStrength,string targetAnim = null)
    {
        ChangeAnim(attackDirection);
        //得到关于自身的攻击方向
    }

    private void ChangeAnim(Vector3 attackDirection)
    {
        Vector3 attackDirecFrom = transform.InverseTransformDirection(attackDirection);
        characterActor.Animator.SetFloat("attackXFrom", attackDirecFrom.x);
        characterActor.Animator.SetFloat("attackYFrom", attackDirecFrom.y);
        characterActor.Animator.SetFloat("attackZFrom", attackDirecFrom.z);
    }
}
