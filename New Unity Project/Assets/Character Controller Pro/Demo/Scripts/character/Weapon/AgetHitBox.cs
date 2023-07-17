using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����н�
/// </summary>
public class AgetHitBox : MonoBehaviour
{
     IAgent target;
    public GameObject agent;
    public List<WeaponManager> manager;
    public CharacterInfo characterInfoOwner;
    private void Awake()
    {
        //�ҵ����������
        characterInfoOwner = gameObject.GetComponentInParent<CharacterInfo>();
        target = agent.GetComponentInParent<IAgent>();
    }
    public void GetDamageOfCharacter()
    {
        //���ǵ��ܻ�
        Debug.Log("�������ǵ��ܻ�����");
    }
    /// <summary>
    /// �ڻ��е���һ֡����
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="pos"></param>
    public void GetDamage(float damage, Vector3 pos)
    {
        //target.GetDamage(damage, pos);
    }
    /// <summary>
    /// �ڻ��е���һ֡����
    /// </summary>
    /// <param name="weapon"></param>
    public void GetWeapon(WeaponManager weapon)
    {
        if(!manager.Contains(weapon))
        manager.Add(weapon);
    }
}
