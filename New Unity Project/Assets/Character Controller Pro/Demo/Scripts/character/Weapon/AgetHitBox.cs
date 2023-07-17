using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻击中介
/// </summary>
public class AgetHitBox : MonoBehaviour
{
     IAgent target;
    public GameObject agent;
    public List<WeaponManager> manager;
    public CharacterInfo characterInfoOwner;
    private void Awake()
    {
        //找到自身的主人
        characterInfoOwner = gameObject.GetComponentInParent<CharacterInfo>();
        target = agent.GetComponentInParent<IAgent>();
    }
    public void GetDamageOfCharacter()
    {
        //主角的受击
        Debug.Log("调用主角的受击方法");
    }
    /// <summary>
    /// 在击中的那一帧调用
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="pos"></param>
    public void GetDamage(float damage, Vector3 pos)
    {
        //target.GetDamage(damage, pos);
    }
    /// <summary>
    /// 在击中的那一帧调用
    /// </summary>
    /// <param name="weapon"></param>
    public void GetWeapon(WeaponManager weapon)
    {
        if(!manager.Contains(weapon))
        manager.Add(weapon);
    }
}
