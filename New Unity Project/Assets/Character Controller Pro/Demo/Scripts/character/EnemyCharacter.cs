using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : CharacterInfo
{
    public float targetGroupSize = 1;
    public float targetHp = 10000;
    public Hitted CharacterHitted;

    protected override  void Awake()
    {
        base.Awake();
        
        if(characterActor != null)
        CharacterHitted = characterActor.GetComponentInChildren<Hitted>();
    }


    public override void GetDamage(float damage, Vector3 pos, WeaponManager weapon, Collider collider, IAgent.HitKind hit = IAgent.HitKind.ground)
    {
        if(CharacterHitted != null)
        CharacterHitted.GetHitted(weapon, hit, true);
        FxManager.Instance.PlayFx<string[]>(weapon.weaponFx, collider.transform);
    }

    

    public override void HitOther(WeaponManager weaponManager)
    {
        throw new System.NotImplementedException();
    }

    public override void GetDamage(float damage, Vector3 attackDirection, float hitStrength, string targetAnim = null)
    {
        ChangeAnim(attackDirection);
        if(!string.IsNullOrEmpty(targetAnim))
        {
            characterActor.Animator.CrossFadeInFixedTime(targetAnim, 0.1f);
        }
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
