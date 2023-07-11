using Cinemachine;
using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

//����Ҫ����һ������ ���ڹ���hit�¼���ʱ���ж���ǰ�������Ƿ����˵��ˣ���������˵��ˣ���ô��������������ҽ��Һ�Ŀ��Ķ��������ٶȽ���
[RequireComponent(typeof(CinemachineImpulseSource))]
[RequireComponent(typeof(Detection))]
public class WeaponManager : MonoBehaviour
{
    
    Detection[] detections;
    //�Ƿ������
    public bool isOnDetection;
    CharacterActor characterActor;
    public bool isHited;
    private CinemachineImpulseSource impulseSource;
    public float[] impulsePar;
    [Tooltip("��������Ҫװ����һ����Ч")]
    public int FxLoad;

    public ParticleSystem[] HittedFx;

    public List<CharacterInfo> HittedCharacter;


    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        detections = GetComponents<Detection>();
        characterActor= GetComponentInParent<CharacterActor>();
        switch(FxLoad)
        {
            case 1: HittedFx = Resources.Load<FxHelper>("FxHelper").Sword;break;
                case 2: break;
        }
        for(int i = 0; i <detections.Length; i++)
        {
            detections[i].WeaponManagerOwner = this;
        }
        
    }
    public void Update()
    {
        HandleDetection();
        //shake();
       // Debug.Log(Time.timeScale);
    }

    /// <summary>
    /// ��⣬����ڼ�⣬��
    /// </summary>
    void HandleDetection()
    {
        Debug.Log(isOnDetection);
        if(isOnDetection)
        {
            foreach(Detection item in detections)
            {
                foreach (var hit in item.GetDetection(out item.isHited))//����˹�������
                {
                    Debug.Log(hit);
                    AgetHitBox hitted = hit.GetComponent<AgetHitBox>();
                    hitted.GetDamage(1, transform.position);//���ǹ������󲥷Ŷ�����
                    hitted.GetWeapon(this);
                }
                //������ڵ�ǰ��detection����Ŀ�꣬��ô�������Ƿ����Ŀ��Ҳ�ĳ�true��
                if(item.isHited == true)
                {
                    //Impluse();//������������Ļ�������ķ�����
                    isHited = true;
                }
            }
        }
    }
    /// <summary>
    /// �رռ��ķ���
    /// </summary>
    /// <param name="value"></param>
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

    /// <summary>
    /// ������
    /// </summary>
    public void Impluse(int i = 0)
    {
        if (impulsePar.Length >3 && impulsePar[0]==1)
        {
            impulseSource.GenerateImpulse(-this.transform.up);
        }
    }
    /// <summary>
    /// ������������Ĺ�����Ч
    /// </summary>
    /// <param name="HitNum"></param>
    public void PlayHittedFx(int HitNum = 0)
    {
        ParticleSystem particle = HittedFx[0];
        particle.transform.position = this.GetComponentInChildren<WeaponFx>().transform.position;
        HittedFx[0].Play(true);
    }
    /// <summary>
    /// ��Ҫ��һ�����������ķ�����ֻ�ڵ�һ�λ���һ��characterINFOʱ�ŵ��ã���Ҫ������ǻ��е��ж����򣬻��е�collider
    /// </summary>
    public void Hitted()
    {

    }
}
