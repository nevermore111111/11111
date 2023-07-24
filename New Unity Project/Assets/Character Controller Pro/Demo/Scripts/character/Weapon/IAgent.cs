using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAgent
{
    public enum HitKind
    {
        ground = 0
    }
    public void GetDamage(float damage,  Vector3 pos, WeaponManager weapon,Collider collider,HitKind hit = HitKind.ground);
}

