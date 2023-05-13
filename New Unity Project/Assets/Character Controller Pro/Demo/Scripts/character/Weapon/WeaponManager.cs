using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����Ҫ����һ������ ���ڹ���hit�¼���ʱ���ж���ǰ�������Ƿ����˵��ˣ���������˵��ˣ���ô��������������ҽ��Һ�Ŀ��Ķ��������ٶȽ���
 
[RequireComponent(typeof(Detection))]
public class WeaponManager : MonoBehaviour
{
    
    Detection[] detections;
    //�Ƿ������
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
                foreach (var hit in item.GetDetection(out item.isHited))//����˹�������
                {
                    AgetHitBox hitted = hit.GetComponent<AgetHitBox>();
                    hitted.GetDamage(1, transform.position);//���ǹ������󲥷Ŷ�����
                    hitted.GetWeapon(this);
                }
                //������ڵ�ǰ��detection����Ŀ�꣬��ô�������Ƿ����Ŀ��Ҳ�ĳ�true��
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
                //���hit�б������Ƿ����Ҳȫ�����
            }
            isHited=false;//���������ж�Ҳ���
        }
    }

}
