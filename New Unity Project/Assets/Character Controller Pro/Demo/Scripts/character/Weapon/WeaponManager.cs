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
    public WeaponData weaponData;
    public bool isNeedUpdateDirection = true;


    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        characterActor = GetComponentInParent<CharacterActor>();
        weaponOwner = GetComponentInParent<CharacterInfo>();
        weaponData = FindFirstObjectByType <WeaponData>();
        switch (FxLoad)
        {
            case 1: HittedFx = Resources.Load<FxHelper>("FxHelper").AllFx; break;
            case 2: break;
        }
        switch (kind)
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
                    detections = GetComponentsInChildren<Detection>().Where(_ => _.WeaponDetector == WeaponDetector.sword).ToArray();
                    break;
                }
            case WeaponKind.fist:
                {
                    detections = GetComponentsInChildren<Detection>().Where(_ => _.WeaponDetector == WeaponDetector.rightFoot || _.WeaponDetector == WeaponDetector.letfFoot || _.WeaponDetector == WeaponDetector.rightHand || _.WeaponDetector == WeaponDetector.leftHand).ToArray();
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
        if (isActiveAndEnabled&&isNeedUpdateDirection)
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
        SetPos();
        //shake();
        // Debug.Log(Time.timeScale);
    }

    private void SetPos()
    {
        weaponData.gameObject.transform.position = transform.position;
        weaponData.transform.forward = transform.forward;
    }

    /// <summary>
    /// �����ײ������ڼ��
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
    /// ��������
    /// </summary>
    /// <param name="impulseRank"></param>
    //public void ChangeDirection(float impulseRank)
    //{
    //    WeaponDirection = weaponData.transform.forward;
    //    Shake(impulseRank);
    //}

    public void Shake(float impulseRank)
    {
        impulseSource.GenerateImpulse(impulseRank * WeaponDirection);
    }

    /// <summary>
    /// ��Ҫ��timeline�������У�һ�������� �� ���ݷ��������С��������ʱ��
    ///// </summary>
    //public void ImplusePlus()
    //{
    //    //�µ��𶯵ķ������޸��𶯴�С������
    //    Impluse(weaponData);
    //}
    //public void Impluse(WeaponData weaponData)
    //{
    //    weaponData.Duration = weaponData.Duration;
    //    weaponData.ImpulseForce = weaponData.ImpulseForce;
    //    // Debug.Log(impulseSource.m_ImpulseDefinition.m_ImpulseDuration);
    //    impulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.Duration;
    //    impulseSource.GenerateImpulse(weaponData.ImpulseForce *brain.transform.InverseTransformDirection( weaponData.transform.TransformVector(weaponData.ImpulseDirection)));

    //}

    /// <summary>
    /// ������
    /// </summary>
    public void SPImpluse(string attackName)
    {
        switch(attackName)
        {
            case "sp11":
                {
                    impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
                    impulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.sp11Duration;
                    impulseSource.GenerateImpulse(WeaponDirection * weaponData.sp11Force);
                    break;
                }
        }
    }


    /// <summary>
    /// ������
    /// </summary>
    public void Impluse(int i = 0)
    {
        if (weaponOwner is MainCharacter)
        {
            //if(impulseSource.m_ImpulseDefinition.m_ImpulseShape != CinemachineImpulseDefinition.ImpulseShapes.Explosion)
            //{
            //    impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
            //}
            impulseSource.m_ImpulseDefinition.m_ImpulseShape = weaponData.ImpulseShapes;

            if (weaponData.onlyUseVirticalShake)
            {
                //��������ֻ���·������
                //������Ҫ�޸ĵ�ǰ��weaponDirection=>
                WeaponDirection = Vector3.Project(WeaponDirection, Vector3.up).normalized;
            }

            switch (weaponOwner.HitStrength)
            {
                case 0:
                    {
                       
                        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.durationValue0;
                        impulseSource.GenerateImpulse(weaponData.impulseValue0 * WeaponDirection);
                        if (weaponData.PrintHit)
                        {
                            Debug.Log($"��ǰ�Ĺ���������{weaponOwner.HitStrength},����ʱ��{impulseSource.m_ImpulseDefinition.m_ImpulseDuration},����{weaponData.impulseValue0}");
                        }
                        break;
                    }
                case 1:
                    {
                        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.durationValue1;
                        impulseSource.GenerateImpulse(weaponData.impulseValue1 * WeaponDirection);
                        if (weaponData.PrintHit)
                        {
                            Debug.Log($"��ǰ�Ĺ���������{weaponOwner.HitStrength},����ʱ��{impulseSource.m_ImpulseDefinition.m_ImpulseDuration},����{weaponData.impulseValue1}");
                        }
                        break;
                    }
                case 2:
                    {
                        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.durationValue2;
                        impulseSource.GenerateImpulse(weaponData.impulseValue2 * WeaponDirection);
                        if (weaponData.PrintHit)
                        {
                            Debug.Log($"��ǰ�Ĺ���������{weaponOwner.HitStrength},����ʱ��{impulseSource.m_ImpulseDefinition.m_ImpulseDuration},����{weaponData.impulseValue2}");
                        }
                        break;
                    }
                case 3:
                    {
                        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.durationValue3;
                        impulseSource.GenerateImpulse(weaponData.impulseValue3 * WeaponDirection);
                        if (weaponData.PrintHit)
                        {
                            Debug.Log($"��ǰ�Ĺ���������{weaponOwner.HitStrength},����ʱ��{impulseSource.m_ImpulseDefinition.m_ImpulseDuration},����{weaponData.impulseValue3}");
                        }
                        break;
                    }
                case 4:
                    {
                        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.durationValue4;
                        impulseSource.GenerateImpulse(weaponData.impulseValue4 * WeaponDirection);
                        if (weaponData.PrintHit)
                        {
                            Debug.Log($"��ǰ�Ĺ���������{weaponOwner.HitStrength},����ʱ��{impulseSource.m_ImpulseDefinition.m_ImpulseDuration},����{weaponData.impulseValue4}");
                        }
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
        Debug.Log("���Ż�����Ч");
        //if (HittedFx[0]!=null)
        //{
        //    ParticleSystem particle = HittedFx[0];
        //    particle.transform.position = this.GetComponentInChildren<WeaponFx>().transform.position;
        //    HittedFx[0].Play(true);
        //}
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
