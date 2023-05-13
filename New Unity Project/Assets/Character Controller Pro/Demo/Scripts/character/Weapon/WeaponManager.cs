using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//我需要做的一个功能 ，在攻击hit事件的时候判定当前的武器是否集中了敌人，如果集中了敌人，那么就震动摄像机，而且将我和目标的动画播放速度降低
 
[RequireComponent(typeof(Detection))]
public class WeaponManager : MonoBehaviour
{
    
    Detection[] detections;
    //是否开启检测
    public bool isOnDetection;
    CharacterActor characterActor;
    public bool isHited;


    private void Awake()
    {
        detections = GetComponents<Detection>();
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
                foreach (var hit in item.GetDetection(out item.isHited))//添加了攻击对象
                {
                    AgetHitBox hitted = hit.GetComponent<AgetHitBox>();
                    hitted.GetDamage(1, transform.position);//这是攻击对象播放都动画
                    hitted.GetWeapon(this);
                }
                //如果存在当前的detection击中目标，那么将武器是否击中目标也改成true。
                if(item.isHited == true)
                {
                    isHited = true;
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
                //清空hit列表，所有是否击中也全部清空
            }
            isHited=false;//武器击中判定也清空
        }
    }

}
