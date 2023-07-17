using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class CharacterInfo : MonoBehaviour, IAgent
{
    //周围全部的敌人
    
    [Tooltip("代表这个单位的敌人tag")]
    public string enemyTag;
    [Tooltip("代表这个单位的敌人列表")]
    public List<CharacterInfo> enemies;
    [Tooltip("这个单位的选择目标")]
    public CharacterInfo selectEnemy;
    [Tooltip("代表摄像机中的碰撞体积")]
    public SphereCollider characterSphere;

    /// <summary>
    /// 伤害，目标位置，武器方向，击中类型
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="pos"></param>
    /// <param name="weapon"></param>
    /// <param name="hit"></param>
    abstract public void GetDamage(float damage, Vector3 pos, WeaponManager weapon,IAgent.HitKind hit = IAgent.HitKind.ground);

 

    protected virtual void  Awake()
    {
        enemies = new List<CharacterInfo>();
        characterSphere = GetComponent<SphereCollider>();
        characterSphere.isTrigger = true;
    }
    
}
