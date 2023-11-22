using Cinemachine.Utility;
using Lightbug.CharacterControllerPro.Implementation;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class Hitted : CharacterState
{
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
    public void GetHitted(WeaponManager weapon,IAgent.HitKind hitKind,bool NeedChangeState = true)
    {
        Debug.Log("�ܻ���");
        
        int hitStrength = weapon.weaponOwner.HitStrength;
        CharacterActor.Velocity = Vector3.zero;
        CharacterActor.RigidbodyComponent.AddForce(HittedForce*(weapon.weaponOwner.transform.position - CharacterActor.transform.position).ProjectOntoPlane(Vector3.up).normalized);
        CharacterActor.RigidbodyComponent.LinearDrag = HittedDrag;
        Debug.Log($"�ٶ�{CharacterActor.Velocity}");
        switch (hitKind)
        {
            case 0:
                {
                    SetAnimationParameters(weapon,true);
                }
                break;
        }
        //�Ƿ���Ҫ�л�״̬����ǰ״̬
        if(NeedChangeState)
        {
            CharacterStateController.EnqueueTransition<Hitted>();
            CharacterActor.Animator.CrossFadeInFixedTime("Hitted.HittedOnGround",0.1f, 0,0.2f);
        }
        else
        {
            CharacterActor.Animator.Play("MixHitted", 1, 0.3f);
            CharacterActor.Animator.SetLayerWeight(1, HittedMixWeight);
        }
        //�Ƿ�������
        if(CharacterActor.isPlayer)
        {

        }
        else
        {

        }
        //����������
    }


    //public Vector3 GetAttackDirection(Transform characterTransform, Vector3 attackSource)
    //{
    //    //Vector3 direction = attackSource - characterTransform.position;
    //    //direction.y = 0f; // ���Ը߶Ȳ�
    //    //direction.Normalize();
    //    Vector3 attackFrom = characterTransform.InverseTransformDirection(-attackSource);
    //    //attackFrom.y = 0f;
    //    attackFrom.Normalize();
    //    return attackFrom;
    //}
    //public Vector2 ConvertToVector2(Vector3 vector3)
    //{
    //    return new Vector2(vector3.x, vector3.z).normalized;
    //}
    //public void SetAnimationParameters(Vector2 attackDirection)
    //{
    //    float attackXFrom = attackDirection.x;
    //    float attackZFrom = attackDirection.y;

    //    // ���ö���������
        
    //    CharacterActor.Animator.SetFloat("attackXFrom", attackXFrom);
    //    CharacterActor.Animator.SetFloat("attackZFrom", attackZFrom);
    //}
    //public void SetAnimationParameters(Vector3 attackDirection)
    //{
    //    float attackXFrom = Mathf.Abs(attackDirection.x);
    //    float attackZFrom = Mathf.Abs(attackDirection.z);
    //    float attackYFrom = Mathf.Abs(attackDirection.y);

    //    // ���ö���������
    //    CharacterActor.Animator.SetFloat("attackXFrom", attackXFrom);
    //    CharacterActor.Animator.SetFloat("attackZFrom", attackZFrom);
    //    CharacterActor.Animator.SetFloat("attackYFrom", attackYFrom);
    //}
    /// <summary>
    /// ���ݶԷ� �����ķ�������������ܻ�����
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="IgnoreYAxis"></param>
    public void SetAnimationParameters(WeaponManager weapon,bool IgnoreYAxis =true)
    {
        Vector3 attackFrom = -TransformHelper.ConvertVector(weapon.WeaponDirection, transform, transform);
        if(IgnoreYAxis)
        {
            attackFrom.y = 0f;
        }
        attackFrom.Normalize();
        CharacterActor.Animator.SetFloat("attackXFrom", attackFrom.x);
        CharacterActor.Animator.SetFloat("attackYFrom", attackFrom.y);
        CharacterActor.Animator.SetFloat("attackZFrom", attackFrom.z);
    }
}
