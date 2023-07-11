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

    public void GetHitted(Vector3 attackSource)
    {
        //动画机处理
        Vector3 attackPos = GetAttackDirection(CharacterActor.transform, attackSource);
        SetAnimationParameters(ConvertToVector2(attackPos));
    }


    public Vector3 GetAttackDirection(Transform characterTransform, Vector3 attackSource)
    {
        //Vector3 direction = attackSource - characterTransform.position;
        //direction.y = 0f; // 忽略高度差
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

        // 设置动画机参数
        
        CharacterActor.Animator.SetFloat("attackXFrom", attackXFrom);
        CharacterActor.Animator.SetFloat("attackZFrom", attackZFrom);
    }
}
