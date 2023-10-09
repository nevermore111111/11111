using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : CharacterInfo
{
     CharacterActor characterActor;
    public float targetGroupSize = 1;
    public float targetHp = 10000;
    public Hitted CharacterHitted;

    protected override  void Awake()
    {
        base.Awake();
        characterActor = this.GetComponentInParent<CharacterActor>();
        if(characterActor != null)
        CharacterHitted = characterActor.GetComponentInChildren<Hitted>();
    }


    public override void GetDamage(float damage, Vector3 pos, WeaponManager weapon, Collider collider, IAgent.HitKind hit = IAgent.HitKind.ground)
    {
        if(CharacterHitted != null)
        CharacterHitted.GetHitted(weapon, hit, true);
    }

    public override void HitOther(WeaponManager weaponManager)
    {
        throw new System.NotImplementedException();
    }
}
