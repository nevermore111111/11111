using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AgetHitBox))]
public abstract class CharacterInfo : MonoBehaviour, IAgent
{


    //��Χȫ���ĵ���
    public CharacterActor characterActor;

    [Tooltip("���������λ�ĵ���tag")]
    public string enemyTag;
    [Tooltip("���������λ�ĵ����б�")]
    public List<CharacterInfo> enemies;
    [Tooltip("�����λ��ѡ��Ŀ��")]
    public CharacterInfo selectEnemy;
    [Tooltip("���������������еĴ�С�뾶��cinemachine��TargetGrounp�е�size")]
    public float cameraRadius = 0.5f;

    public AgetHitBox hitBox;
    public int HitStrength;
    public bool canBeExecuted = false;
    /// <summary>
    /// �˺���Ŀ��λ�ã��������򣬻�������
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="pos"></param>
    /// <param name="weapon"></param>
    /// <param name="collider">�ܻ�λ��</param>
    /// <param name="hit"></param>
    abstract public void GetDamage(float damage, Vector3 pos, WeaponManager weapon, Collider collider, IAgent.HitKind hit = IAgent.HitKind.ground);

    virtual public void GetDamage(float damage, Vector3 attackDirection, float hitStrength,string targetAnim = null)
    {

    }




    protected virtual void Awake()
    {
        enemies = new List<CharacterInfo>();
        hitBox = GetComponentInChildren<AgetHitBox>();
        characterActor = GetComponentInParent<CharacterActor>();
    }

    abstract public void HitOther(WeaponManager weaponManager);

}
