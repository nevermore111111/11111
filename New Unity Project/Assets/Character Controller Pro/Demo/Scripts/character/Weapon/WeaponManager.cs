using Cinemachine;
using Lightbug.CharacterControllerPro.Core;
using MagicaCloth2;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

//����Ҫ����һ������ ���ڹ���hit�¼���ʱ���ж���ǰ�������Ƿ����˵��ˣ���������˵��ˣ���ô��������������ҽ��Һ�Ŀ��Ķ��������ٶȽ���
[RequireComponent(typeof(CinemachineImpulseSource))]
[RequireComponent(typeof(Detection))]
public class WeaponManager : MonoBehaviour
{

    public enum WeaponKind
    {
        nullArm,
        sword,
        fist
    }
    public WeaponKind kind;
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
    public Vector3 WeaponDirection;
    private int frameCount = 0;
    private Vector3 previousWeaponPosition;
    public CharacterInfo weaponOwner;


    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        detections = GetComponents<Detection>();
        characterActor = GetComponentInParent<CharacterActor>();
        weaponOwner = GetComponentInParent<CharacterInfo>();
        switch (FxLoad)
        {
            case 1: HittedFx = Resources.Load<FxHelper>("FxHelper").Sword; break;
            case 2: break;
        }
        for (int i = 0; i < detections.Length; i++)
        {
            detections[i].Weapon = this;
        }

    }
    //�ڼ����У���ÿ��֡����һ�������������
    public void UpdateWeaponDirection()
    {
        if (isActiveAndEnabled)
        {
            if (frameCount == 1)
            {
                // ��ȡ��ǰ����λ��
                Vector3 currentWeaponPosition = transform.position;

                if (currentWeaponPosition - previousWeaponPosition != Vector3.zero)
                // ������������

                {
                    WeaponDirection = currentWeaponPosition - previousWeaponPosition;


                    WeaponDirection.Normalize();

                    previousWeaponPosition = currentWeaponPosition;
                }

                // ������һ֡������λ��
            }

            // ����֡����
            frameCount = (frameCount + 1) % 2;
        }
    }
    public void Update()
    {
        HandleDetection();
        UpdateWeaponDirection();
        //shake();
        // Debug.Log(Time.timeScale);
    }

    /// <summary>
    /// ��⣬����ڼ�⣬��
    /// </summary>
    void HandleDetection()
    {

        if (isOnDetection)
        {
            foreach (Detection item in detections)
            {
                foreach (var hit in item.GetDetection(out item.isHited))//����˹�������
                {

                    AgetHitBox hitted = hit.GetComponent<AttackReceive>().CharacterInfo.hitBox;
                    //hitted.GetDamage(1, transform.position);//���ǹ������󲥷Ŷ�����
                    hitted.GetWeapon(this);
                }
                //������ڵ�ǰ��detection����Ŀ�꣬��ô�������Ƿ����Ŀ��Ҳ�ĳ�true��
                if (item.isHited == true)
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
            foreach (var item in detections)
            {
                item.ClaerWasHit();
                //���hit�б������Ƿ����Ҳȫ�����

            }
            isHited = false;//���������ж�Ҳ���
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    public void Impluse(int i = 0)
    {
        if (weaponOwner is MainCharacter)
        {
            switch (weaponOwner.HitKind)
            {
                case 0:
                    {
                        impulseSource.GenerateImpulse(0.4f*WeaponDirection);
                        break;
                    }
                case 1:
                    {
                        impulseSource.GenerateImpulse(0.6f * WeaponDirection);
                        break;
                    }
                case 2:
                    {
                        impulseSource.GenerateImpulse(0.9f * WeaponDirection);
                        break;
                    }
            }

            
        }

        else
        {

        }
    }
    /// <summary>
    /// ������������Ĺ�����Ч
    /// </summary>
    /// <param name="HitNum"></param>
    public void PlayHittedFx(int HitNum = 0)
    {
        //ParticleSystem particle = HittedFx[0];
        //particle.transform.position = this.GetComponentInChildren<WeaponFx>().transform.position;
        //HittedFx[0].Play(true);
    }
    /// <summary>
    /// ���Ż�����Ч
    /// </summary>
    public void PlayFX()
    {

    }

    /// <summary>
    /// һ��ʱͣ���𶯵ĸ��Ϸ�����
    /// </summary>
    /// <param name="fadeInDuration"></param>
    /// <param name="fadeOutDuration"></param>
    /// <param name="duration"></param>
    /// <param name="targetTimeScale"></param>
    /// <param name="weaponManager"></param>
    /// <returns></returns>

    /// <summary>
    /// �������������ϵ�������˶�����
    /// </summary>
    /// <returns></returns>
    public Vector3 GetWeaponDirectInverse(Transform HittedCharacterTransform)
    {
        return HittedCharacterTransform.InverseTransformDirection(WeaponDirection);
    }
}
