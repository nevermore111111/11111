using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    
    public List<Detection> detections = new List<Detection>();
    public bool isOnDetection;
    public CharacterActor characterActor;

    private void Awake()
    {
        characterActor= GetComponentInParent<CharacterActor>();
    }
    public void Update()
    {
        HandleDetection();
    }
    void HandleDetection()
    {
        if(isOnDetection)
        {
            foreach(Detection item in detections)
            {
                foreach (var hit in item.GetDetection())//添加了攻击对象
                {
                    AgetHitBox hitted = hit.GetComponent<AgetHitBox>();
                    hitted.GetDamage(1, transform.position);//这是攻击对象播放都动画
                    hitted.GetWeapon(this);
                }
            }
        }
    }
    public void ToggleDetection(bool value)
    {
        isOnDetection = value;
        if (isOnDetection)
        {
            HandleDetection();
        }
        else
        {
            foreach(var item in detections)
            {
                item.ClaerWasHit();
            }
        }
    }

}
