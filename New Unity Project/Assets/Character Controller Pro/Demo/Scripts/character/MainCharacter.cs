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
        //��Ҫ�ҵ����ǵ���
        CharacterHitted.GetHitted( weapon,hit );
    }


    protected override void Awake()
    {
        base.Awake();
        CharacterActor = GetComponentInParent<CharacterActor>();
        CharacterStateController = this.transform.parent.GetComponentInChildren<CharacterStateController>();
        CharacterHitted = GetComponentInChildren<Hitted>();
    }

#warning(����û��,�����ʹ�õ�)
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
        // ����
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / fadeInDuration);
            Time.timeScale = Mathf.Lerp(initialTimeScale, targetTimeScale, normalizedTime);
            // ���������������Ҫ�����������߼�����
            // �ȴ�һ֡
            yield return null;
        }
        // ����Ŀ��ʱ������
        Time.timeScale = targetTimeScale;
        if(weaponManager.isActiveAndEnabled)
        {
            weaponManager.PlayHittedFx();
        }
        // ����ʱ��
        yield return new WaitForSecondsRealtime(duration);

        // ����
        elapsedTime = 0f;
        if (weaponManager.isActiveAndEnabled)
        {
      
            //�����𶯺���Ч
            weaponManager.Impluse();
        }
        //������Ҫ���������ط�������Ч��һ��������ĵ��������Ч������һ���ǹ�����ܻ�������
        //��Ҫ����ί��


        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / fadeOutDuration);
            Time.timeScale = Mathf.Lerp(targetTimeScale, 1f, normalizedTime);
            // ���������������Ҫ�����������߼�����

            // �ȴ�һ֡
            yield return null;
        }

        //һ����ʱͣ�����ʱ����ȥ�������������Ч����
        // �ָ�ԭʼ��ʱ������
        Time.timeScale = 1f;
    }
}
