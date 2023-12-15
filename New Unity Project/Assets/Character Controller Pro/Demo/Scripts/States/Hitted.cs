using Cinemachine.Utility;
using Lightbug.CharacterControllerPro.Implementation;
using OfficeOpenXml.Packaging;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class Hitted : CharacterState
{
    public bool debug = false;
    [Space(10f)]

    public float HittedForce = 10f;
    public float HittedDrag = 2f;
    public float HittedMixWeight = 0.5f;
    override protected void Start()
    {
        base.Start();
    }
    protected override void Awake()
    {
        base.Awake();
    }
    public override void UpdateBehaviour(float dt)
    {

    }
    public override void EnterBehaviour(float dt, CharacterState fromState)
    {
        Debug.Log("�������ܻ�״̬");
        if (CharacterStateController.CurrentState is AttackOnGround)
        {

        }
        if (CharacterActor.isPlayer)
        {

        }
    }
    //����Ŀ��ķ�λ�͹�������������������ܻ����͡���Ҫ���õ�ǰ���ܻ�������
    public void GetHitted(WeaponManager weapon, IAgent.HitKind hitKind, bool NeedChangeState = true)
    {
        Debug.Log("�ܻ���");
        int hitStrength = weapon.weaponOwner.HitStrength;
        HittedBack(weapon, true);
        //����
        CheckAnimator(weapon, hitKind, NeedChangeState);
        //����������
    }
    public void GetHitted(Vector3 attackDirection,Vector3 hittedForce  ,bool NeedChangeState = true)
    {
        Debug.Log("�ܻ���");


        CharacterActor.RigidbodyComponent.Velocity = Vector3.zero;
        CharacterActor.RigidbodyComponent.AddForce(hittedForce);
    }

    private void CheckAnimator(WeaponManager weapon, IAgent.HitKind hitKind, bool NeedChangeState)
    {
        Vector3 HitWorldDir = weapon.weaponOwner.transform.TransformDirection(weapon.WeaponDirection);
        if(debug)
        {
            Debug.DrawLine(weapon.transform.position, weapon.transform.position + HitWorldDir, Color.blue);
            Time.timeScale = 0f;
        }

        SetAnimationParameters(HitWorldDir, true);
        //�Ƿ���Ҫ�л�״̬����ǰ״̬
        if (NeedChangeState)
        {
            CharacterStateController.EnqueueTransition<Hitted>();
            CharacterActor.Animator.CrossFadeInFixedTime("Hitted.HittedOnGround", 0.1f, 0, 0.2f);
        }
        else
        {
            CharacterActor.Animator.Play("MixHitted", 1, 0.3f);
            CharacterActor.Animator.SetLayerWeight(1, HittedMixWeight);
        }
        PlayerSpecial();
    }

    /// <summary>
    /// �������ⷽ��
    /// </summary>
    private void PlayerSpecial()
    {
        //�Ƿ�������
        if (CharacterActor.isPlayer)
        {

        }
        else
        {

        }
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="resetVelocity"></param>
    private void HittedBack(WeaponManager weapon, bool resetVelocity)
    {
        if (resetVelocity)
        {
            CharacterActor.Velocity = Vector3.zero;
        }
        Vector3 targetMove = HittedForce * (weapon.weaponOwner.transform.position - CharacterActor.transform.position).ProjectOntoPlane(Vector3.up).normalized;
        CharacterActor.RigidbodyComponent.AddForce(targetMove);
        CharacterActor.RigidbodyComponent.LinearDrag = HittedDrag;
    }



    /// <summary>
    /// ���ݶԷ� �����ķ�������������ܻ�����
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="IgnoreYAxis"></param>
    public void SetAnimationParameters(Vector3 WorldAttackDirection, bool IgnoreYAxis = true)
    {
        Vector3 attackFrom = this.transform.InverseTransformDirection(WorldAttackDirection);
        if (IgnoreYAxis)
        {
            attackFrom.y = 0f;
        }
        attackFrom.Normalize();
        CharacterActor.Animator.SetFloat("attackXFrom", attackFrom.x);
        CharacterActor.Animator.SetFloat("attackYFrom", attackFrom.y);
        CharacterActor.Animator.SetFloat("attackZFrom", attackFrom.z);
    }
}
