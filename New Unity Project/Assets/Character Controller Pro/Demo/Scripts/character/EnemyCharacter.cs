using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : CharacterInfo
{
     CharacterActor characterActor;
    public float targetGroupSize = 1;
    public float targetHp = 10000;
    public BlazeAI BlazeAI;

    protected override  void Awake()
    {
        base.Awake();
        BlazeAI = GetComponentInChildren<BlazeAI>();
       // characterActor = GetComponentInParent<CharacterActor>();
    }


    public override void GetDamage(float damage, Vector3 pos, WeaponManager weapon, Collider collider, IAgent.HitKind hit = IAgent.HitKind.ground)
    {
        
    }

    public override void HitOther(WeaponManager weaponManager)
    {
        throw new System.NotImplementedException();
    }
}
