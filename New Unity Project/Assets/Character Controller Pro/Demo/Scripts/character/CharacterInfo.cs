using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Implementation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AgetHitBox))]
public abstract class CharacterInfo : MonoBehaviour, IAgent
{

    //周围全部的敌人
    public CharacterActor characterActor;
    public CharacterStateController CharacterStateController { get; private set; }


    [Tooltip("代表这个单位的敌人tag")]
    public string enemyTag;
    [Tooltip("代表这个单位的敌人列表")]
    public List<CharacterInfo> enemies;
    [Tooltip("这个单位的选择目标")]
    public CharacterInfo selectEnemy;
    [Tooltip("这个人物在摄像机中的大小半径，cinemachine的TargetGrounp中的size")]
    public float cameraRadius = 0.5f;

    public AgetHitBox hitBox;
    public int HitStrength;
    public bool canBeExecuted = false;

    [HideInInspector]
    public List<AttackReceive> allReceives;
    [HideInInspector]
    public List<SkillReceiver> allSkillReceivers;

    public AttackInfo attackInfo = new AttackInfo();



    /// <summary>
    /// 伤害，目标位置，武器方向，击中类型
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="pos"></param>
    /// <param name="weapon"></param>
    /// <param name="collider">受击位置</param>
    /// <param name="hit"></param>
    abstract public void GetDamage(float damage, Vector3 pos, WeaponManager weapon, Collider collider, IAgent.HitKind hit = IAgent.HitKind.ground);

    virtual public void GetDamage(float damage, Vector3 attackDirection, float hitStrength, string targetAnim = null)
    {

    }

    public SkillReceiver GetSkillReceiver(int requireSkillPointNum)
    {
        return allSkillReceivers.FirstOrDefault(_ => _.skillPoint == requireSkillPointNum);
    }


    protected virtual void Awake()
    {
        enemies = new List<CharacterInfo>();
        hitBox = GetComponentInChildren<AgetHitBox>();
        characterActor = GetComponentInParent<CharacterActor>();
        CharacterStateController = this.transform.parent.GetComponentInChildren<CharacterStateController>();
    }

    abstract public void HitOther(WeaponManager weaponManager);

}
