using Cinemachine;
using Lightbug.CharacterControllerPro.Core;
using MagicaCloth2;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEditor;
using UnityEngine;

//����Ҫ����һ������ ���ڹ���hit�¼���ʱ���ж���ǰ�������Ƿ����˵��ˣ���������˵��ˣ���ô��������������ҽ��Һ�Ŀ��Ķ��������ٶȽ���
//[RequireComponent(typeof(CinemachineImpulseSource))]
//[RequireComponent(typeof(Detection))]
public class WeaponManager : MonoBehaviour
{


    public WeaponKind kind;//������������࣬������������������ȥ��Ӧ������Щ̽����
    Detection[] detections;    //�������������̽����
    public WeaponDetector[] ActiveWeaponDetectors;//���������ǰ�����̽����
    public bool isOnDetection;  //�Ƿ������
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
        characterActor = GetComponentInParent<CharacterActor>();
        weaponOwner = GetComponentInParent<CharacterInfo>();
        switch (FxLoad)
        {
            case 1: HittedFx = Resources.Load<FxHelper>("FxHelper").Sword; break;
            case 2: break;
        }
        switch(kind)
        {
            default:
                {
                    Debug.LogError("���weaponû��ѡ������");
                    break;
                }
            case WeaponKind.sword:
                {
                    #region(ѧϰ)
                    /*
                     * 
Where �� Select �� LINQ�����Լ��ɲ�ѯ���е��������ò����������ڶԼ��ϣ������顢�б���ѯ����ȣ�����ɸѡ��ת�������ǵ���;�͹���������ͬ��

Where��
Where ����������ɸѡ�����е�Ԫ�أ����������ض�������Ԫ���Ӽ���������һ��������ν�ʣ���Ϊ������������һ���µļ��ϣ����а�������������Ԫ�ء�

ʾ����

csharp
Copy code
var evenNumbers = numbers.Where(x => x % 2 == 0);
������ʾ���У�numbers ��һ���������ϣ�Where ����ɸѡ�����е�ż��Ԫ�ء�

Select��
Select ���������ڽ������е�ÿ��Ԫ��ת������һ�����ͣ��γ�һ���µļ��ϡ�������һ��ת��������Ϊ������������һ���µļ��ϣ����а���Ӧ��ת��������Ľ����

ʾ����

csharp
Copy code
var squaredNumbers = numbers.Select(x => x * x);
                     */
                    #endregion
                    //����where��˵��ѡ��Ļ���ԭ����ֵ������select��˵�����ص���һ���µĶ���
                    detections = GetComponentsInChildren<Detection>().Where(_=> _.WeaponDetector == WeaponDetector.sword).ToArray();
                    break;
                }
                case WeaponKind.fist:
                {
                    detections = GetComponentsInChildren<Detection>().Where(_ => _.WeaponDetector == WeaponDetector.rightFoot|| _.WeaponDetector == WeaponDetector.letfFoot|| _.WeaponDetector == WeaponDetector.rightHand|| _.WeaponDetector == WeaponDetector.leftHand).ToArray();
                    break;
                }
        }
        
               
        
    }

   

    public void AdjustFrequencyAndAmplitude(float frequency, float amplitude)
    {
        if (impulseSource != null)
        {
            // ����Ƶ�ʺ�����
            impulseSource.m_ImpulseDefinition.m_FrequencyGain = frequency;
            impulseSource.m_ImpulseDefinition.m_AmplitudeGain = amplitude;
        }
    }


    /// <summary>
    ///  �ڼ����У���ÿ��֡����һ�������������
    /// </summary>
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
                //�����ǰ��������岻Ӧ�ð��������ôֱ������������һ��
                if (!this.ActiveWeaponDetectors.Contains(item.WeaponDetector))
                {
                    continue;
                }
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
                        impulseSource.GenerateImpulse(0.4f * WeaponDirection);
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
        ParticleSystem particle = HittedFx[0];
        particle.transform.position = this.GetComponentInChildren<WeaponFx>().transform.position;
        HittedFx[0].Play(true);
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
public enum WeaponKind
{
    nullArm,
    sword,
    fist
}
