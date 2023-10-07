using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AgetHitBox))]
public abstract class CharacterInfo : MonoBehaviour, IAgent
{
    //周围全部的敌人
    
    [Tooltip("代表这个单位的敌人tag")]
    public string enemyTag;
    [Tooltip("代表这个单位的敌人列表")]
    public List<CharacterInfo> enemies;
    [Tooltip("这个单位的选择目标")]
    public CharacterInfo selectEnemy;
    [Tooltip("这个人物在摄像机中的大小半径，cinemachine的TargetGrounp中的size")]
    public float cameraRadius = 0.5f;

    public AgetHitBox hitBox;
    public int HitKind;
    /// <summary>
    /// 伤害，目标位置，武器方向，击中类型
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="pos"></param>
    /// <param name="weapon"></param>
    /// <param name="collider">受击位置</param>
    /// <param name="hit"></param>
    abstract public void GetDamage(float damage, Vector3 pos, WeaponManager weapon,Collider collider,IAgent.HitKind hit = IAgent.HitKind.ground);

 

    protected virtual void  Awake()
    {
        enemies = new List<CharacterInfo>();
        hitBox = GetComponentInChildren<AgetHitBox>();
    }

    abstract public void HitOther( WeaponManager weaponManager);
    
}
