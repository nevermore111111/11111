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
    private void Awake()
    {
        target = agent.GetComponent<IAgent>();
    }
    public void GetDamage(float damage,Vector3 pos)
    {
        target.GetDamage(damage, pos);
    }
}
