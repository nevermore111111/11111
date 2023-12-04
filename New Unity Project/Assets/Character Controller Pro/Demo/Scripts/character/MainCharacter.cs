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
    public HitData hitData;

    public override void GetDamage(float damage, Vector3 pos, WeaponManager weapon, Collider collider, IAgent.HitKind hit = IAgent.HitKind.ground)
    {
        //��Ҫ�ҵ����ǵ���
        CharacterHitted.GetHitted(weapon, hit);
    }


    protected override void Awake()
    {
        base.Awake();
        CharacterActor = this.GetComponentInParent<CharacterActor>();
        CharacterStateController = this.transform.parent.GetComponentInChildren<CharacterStateController>();
        CharacterHitted = CharacterActor.GetComponentInChildren<Hitted>();
        hitData = FindObjectOfType<HitData>();
    }

#warning(����û��,�����ʹ�õ�)
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

            // ���� NormalMovement ʵ���ı���
            if (normalMovementState != null)
            {

                return normalMovementState.moving;
                // ʹ�� normalMovementState ����ʵ���ı���
                // ����: normalMovementState.SomeVariable
            }
        }
        return false;
    }
    override public void HitOther(WeaponManager weapon)
    {
        //�����Ҫһ������ʱ�䷽����������ȥ������ι����Ĳ���
        //���û���Ч��
        HitParByHitKind(weapon);
    }

    //������Ҵ򵽱��˵ķ���
    public void HitParByHitKind(WeaponManager weapon)
    {

#if UNITY_EDITOR
        // ֻ��Unity�༭�������еĴ���
        if (hitData.ForceCurrentHit != -1)
        {
            //���ǿ��ʹ������𶯣��Ქ������𶯵�����Ч��
            HitStrength = hitData.ForceCurrentHit;
        }
#endif
        HitPlus(HitStrength, hitData, weapon);
    }
    public void HitPlus(int currentHit, HitData hitData, WeaponManager weaponManager)
    {
        float fadeInDuration = hitData.GetFadeTime(hitData, currentHit);
        float fadeOutDuration = hitData.GetFadeTime(hitData, currentHit); ; // �������ʱ��ͽ���ʱ����ͬ
        float duration = hitData.GetStayTime(hitData, currentHit);
        float targetTimeScale = hitData.GetTimeScale(hitData, currentHit);
        TimeScaleManager.Instance.SetTimeScale(fadeInDuration, fadeOutDuration, duration, targetTimeScale);
        if (weaponManager.isActiveAndEnabled)
        {
            //�����𶯺���Ч
            weaponManager.Impluse();
        }
    }

    //public IEnumerator Hit(int currentHit, HitData hitData, WeaponManager weaponManager)
    //{
    //    float fadeInDuration = hitData.GetFadeTime(hitData, currentHit);
    //    float fadeOutDuration = hitData.GetFadeTime(hitData, currentHit); ; // �������ʱ��ͽ���ʱ����ͬ
    //    float duration = hitData.GetStayTime(hitData, currentHit);
    //    float targetTimeScale = hitData.GetTimeScale(hitData, currentHit);
    //    float initialTimeScale = Time.timeScale;
    //    float elapsedTime = 0f;
    //    if (weaponManager.isActiveAndEnabled)
    //    {
    //        //�����𶯺���Ч
    //        weaponManager.Impluse();
    //        Debug.LogError("��ʼִ���ܻ�����");
    //    }
    //    // ����
    //    while (elapsedTime < fadeInDuration)
    //    {
    //        elapsedTime += Time.unscaledDeltaTime;
    //        float normalizedTime = Mathf.Clamp01(elapsedTime / fadeInDuration);
    //        Time.timeScale = Mathf.Lerp(initialTimeScale, targetTimeScale, normalizedTime);
    //        // ���������������Ҫ�����������߼�����
    //        // �ȴ�һ֡
    //        yield return null;
    //    }
    //    // ����Ŀ��ʱ������
    //    Time.timeScale = targetTimeScale;
    //    if (weaponManager.isActiveAndEnabled)
    //    {
    //        //weaponManager.PlayHittedFx();
    //    }
    //    // ����ʱ��
    //    yield return new WaitForSecondsRealtime(duration);

    //    // ����
    //    elapsedTime = 0f;

    //    //������Ҫ���������ط�������Ч��һ��������ĵ��������Ч������һ���ǹ�����ܻ�������
    //    //��Ҫ����ί��


    //    while (elapsedTime < fadeOutDuration)
    //    {
    //        elapsedTime += Time.unscaledDeltaTime;
    //        float normalizedTime = Mathf.Clamp01(elapsedTime / fadeOutDuration);
    //        Time.timeScale = Mathf.Lerp(targetTimeScale, 1f, normalizedTime);
    //        // ���������������Ҫ�����������߼�����

    //        // �ȴ�һ֡
    //        yield return null;
    //    }

    //    //һ����ʱͣ�����ʱ����ȥ�������������Ч����
    //    // �ָ�ԭʼ��ʱ������
    //    Time.timeScale = 1f;
    //}



}
