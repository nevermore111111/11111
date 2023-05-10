using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    
    public List<Detection> detections = new List<Detection>();
    public bool isOnDetection;


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
                foreach (var hit in item.GetDetection())
                {
                    if()
                    hit.GetComponent<AgetHitBox>().GetDamage(1, transform.position);
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
