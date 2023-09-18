using Lightbug.CharacterControllerPro.Implementation;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hitted : CharacterState
{
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
        
    }
    //����Ŀ��ķ�λ�͹�������������������ܻ����͡���Ҫ���õ�ǰ���ܻ�������
    public void GetHitted(WeaponManager weapon,IAgent.HitKind hitKind)
    {
        Debug.Log("�����ܻ���");
        switch(hitKind)
        {
            case 0:
                {
                    CharacterStateController.EnqueueTransition<Hitted>();
                    Vector3 attackPos = -weapon.GetWeaponDirectInverse(CharacterActor.transform);
                    SetAnimationParameters(ConvertToVector2(attackPos));
                }
                break;
        }
        //����������
    }


    public Vector3 GetAttackDirection(Transform characterTransform, Vector3 attackSource)
    {
        //Vector3 direction = attackSource - characterTransform.position;
        //direction.y = 0f; // ���Ը߶Ȳ�
        //direction.Normalize();
        Vector3 attackFrom = characterTransform.InverseTransformDirection(-attackSource);
        //attackFrom.y = 0f;
        attackFrom.Normalize();
        return attackFrom;
    }
    public Vector2 ConvertToVector2(Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z).normalized;
    }
    public void SetAnimationParameters(Vector2 attackDirection)
    {
        float attackXFrom = attackDirection.x;
        float attackZFrom = attackDirection.y;

        // ���ö���������
        
        CharacterActor.Animator.SetFloat("attackXFrom", attackXFrom);
        CharacterActor.Animator.SetFloat("attackZFrom", attackZFrom);
    }
    public void SetAnimationParameters(Vector3 attackDirection)
    {
        float attackXFrom = Mathf.Abs(attackDirection.x);
        float attackZFrom = Mathf.Abs(attackDirection.z);
        float attackYFrom = Mathf.Abs(attackDirection.y);

        // ���ö���������
        CharacterActor.Animator.SetFloat("attackXFrom", attackXFrom);
        CharacterActor.Animator.SetFloat("attackZFrom", attackZFrom);
        CharacterActor.Animator.SetFloat("attackYFrom", attackYFrom);
    }
}
