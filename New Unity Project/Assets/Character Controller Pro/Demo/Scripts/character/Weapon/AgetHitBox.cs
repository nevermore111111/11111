using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¹¥»÷ÖÐ½é
/// </summary>
public class AgetHitBox : MonoBehaviour
{
     IAgent target;
    public GameObject agent;
    public List<WeaponManager> manager;
    private void Awake()
    {
        target = agent.GetComponent<IAgent>();
    }
    public void GetDamage(float damage,Vector3 pos)
    {
        target.GetDamage(damage, pos);
    }
    public void GetWeapon(WeaponManager weapon)
    {
        if(!manager.Contains(weapon))
        manager.Add(weapon);
    }
}
