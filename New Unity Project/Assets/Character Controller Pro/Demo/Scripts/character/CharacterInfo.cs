using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CharacterInfo : MonoBehaviour
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
    protected virtual void  Awake()
    {
        enemies = new List<CharacterInfo>();
       //if(TryGetComponent<SphereCollider>(out characterSphere))
        characterSphere = GetComponent<SphereCollider>();
        //if(characterSphere)
        characterSphere.isTrigger = true;
    }
   
    
}
