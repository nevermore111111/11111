using Cinemachine;
using Lightbug.CharacterControllerPro.Core;
using MagicaCloth2;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEditor;
using UnityEngine;

//我需要做的一个功能 ，在攻击hit事件的时候判定当前的武器是否集中了敌人，如果集中了敌人，那么就震动摄像机，而且将我和目标的动画播放速度降低
//[RequireComponent(typeof(CinemachineImpulseSource))]
//[RequireComponent(typeof(Detection))]
public class WeaponManager : MonoBehaviour
{


    public WeaponKind kind;//这个武器的种类，会根据这个武器的种类去那应该有哪些探测器
    Detection[] detections;    //这个武器的所有探测器
    public WeaponDetector[] ActiveWeaponDetectors;//这个武器当前激活的探测器
    public bool isOnDetection;  //是否开启检测
    CharacterActor characterActor;
    public bool isHited;
    private CinemachineImpulseSource impulseSource;
    public float[] impulsePar;
    [Tooltip("这里配置要装载哪一段特效")]
    public int FxLoad;
    public ParticleSystem[] HittedFx;
    public List<CharacterInfo> HittedCharacter;
    public Vector3 WeaponDirection;
    private int frameCount = 0;
    private Vector3 previousWeaponPosition;
    public CharacterInfo weaponOwner;
    public WeaponData weaponData;


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
                    Debug.LogError("这个weapon没有选择种类");
                    break;
                }
            case WeaponKind.sword:
                {
                    #region(学习)
                    /*
                     * 
Where 和 Select 是 LINQ（语言集成查询）中的两个常用操作符，用于对集合（如数组、列表、查询结果等）进行筛选和转换。它们的用途和功能有所不同：

Where：
Where 操作符用于筛选集合中的元素，返回满足特定条件的元素子集。它接受一个条件（谓词）作为参数，并返回一个新的集合，其中包含满足条件的元素。

示例：

csharp
Copy code
var evenNumbers = numbers.Where(x => x % 2 == 0);
在上述示例中，numbers 是一个整数集合，Where 操作筛选出其中的偶数元素。

Select：
Select 操作符用于将集合中的每个元素转换成另一种类型，形成一个新的集合。它接受一个转换函数作为参数，并返回一个新的集合，其中包含应用转换函数后的结果。

示例：

csharp
Copy code
var squaredNumbers = numbers.Select(x => x * x);
                     */
                    #endregion
                    //对于where来说，选择的还是原本的值，对于select来说，返回的是一个新的对象。
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
            // 设置频率和力度
            impulseSource.m_ImpulseDefinition.m_FrequencyGain = frequency;
            impulseSource.m_ImpulseDefinition.m_AmplitudeGain = amplitude;
        }
    }


    /// <summary>
    ///  在激活中，就每三帧更新一次这个武器方向
    /// </summary>
    public void UpdateWeaponDirection()
    {
        if (isActiveAndEnabled)
        {
            if (frameCount == 1)
            {
                // 获取当前武器位置
                Vector3 currentWeaponPosition = transform.position;

                if (currentWeaponPosition - previousWeaponPosition != Vector3.zero)
                // 计算武器方向

                {
                    WeaponDirection = currentWeaponPosition - previousWeaponPosition;


                    WeaponDirection.Normalize();

                    previousWeaponPosition = currentWeaponPosition;
                }

                // 更新上一帧的武器位置
            }

            // 增加帧计数
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
    /// 检测，如果在检测，就
    /// </summary>
    void HandleDetection()
    {

        if (isOnDetection)
        {
            foreach (Detection item in detections)
            {
                //如果当前激活的物体不应该包括这个那么直接跳出进入下一个
                if (!this.ActiveWeaponDetectors.Contains(item.WeaponDetector))
                {
                    continue;
                }
                foreach (var hit in item.GetDetection(out item.isHited))//添加了攻击对象
                {

                    AgetHitBox hitted = hit.GetComponent<AttackReceive>().CharacterInfo.hitBox;
                    //hitted.GetDamage(1, transform.position);//这是攻击对象播放都动画
                    hitted.GetWeapon(this);
                }
                //如果存在当前的detection击中目标，那么将武器是否击中目标也改成true。
                if (item.isHited == true)
                {
                    //Impluse();//这里调用武器的或者人物的方法。
                    isHited = true;
                }
            }
        }
    }
    /// <summary>
    /// 关闭检测的方法
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
                //清空hit列表，所有是否击中也全部清空

            }
            isHited = false;//武器击中判定也清空
        }
    }
    /// <summary>
    /// 调整并震动
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
    /// 需要给timeline增加两列，一调整方向 二 根据方向调整大小，三调整时间
    ///// </summary>
    //public void ImplusePlus()
    //{
    //    //新的震动的方法，修改震动大小并且震动
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
    /// 产生震动
    /// </summary>
    public void SPImpluse(string attackName)
    {
        switch(attackName)
        {
            case "sp11":
                {
                    impulseSource.m_ImpulseDefinition.m_ImpulseShape = weaponData.ImpulseShapes;
                    impulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.sp11Duration;
                    impulseSource.GenerateImpulse(WeaponDirection * weaponData.sp11Force);
                    break;
                }
        }
    }


    /// <summary>
    /// 产生震动
    /// </summary>
    public void Impluse(int i = 0)
    {
        if (weaponOwner is MainCharacter)
        {
            if(impulseSource.m_ImpulseDefinition.m_ImpulseShape != CinemachineImpulseDefinition.ImpulseShapes.Explosion)
            {
                impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
            }

            if(weaponData.onlyUseVirticalShake)
            {
                //这样代表只上下方向的震动 
                //这是需要修改当前的weaponDirection=>
                WeaponDirection = Vector3.Project(WeaponDirection, Vector3.up);
            }

            switch (weaponOwner.HitStrength)
            {
                case 0:
                    {
                       
                        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.durationValue0;
                        impulseSource.GenerateImpulse(weaponData.impulseValue0 * WeaponDirection);
                        if (weaponData.PrintHit)
                        {
                            Debug.Log($"当前的攻击类型是{weaponOwner.HitStrength},持续时间{impulseSource.m_ImpulseDefinition.m_ImpulseDuration},力度{weaponData.impulseValue0}");
                        }
                        break;
                    }
                case 1:
                    {
                        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.durationValue1;
                        impulseSource.GenerateImpulse(weaponData.impulseValue1 * WeaponDirection);
                        if (weaponData.PrintHit)
                        {
                            Debug.Log($"当前的攻击类型是{weaponOwner.HitStrength},持续时间{impulseSource.m_ImpulseDefinition.m_ImpulseDuration},力度{weaponData.impulseValue1}");
                        }
                        break;
                    }
                case 2:
                    {
                        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.durationValue2;
                        impulseSource.GenerateImpulse(weaponData.impulseValue2 * WeaponDirection);
                        if (weaponData.PrintHit)
                        {
                            Debug.Log($"当前的攻击类型是{weaponOwner.HitStrength},持续时间{impulseSource.m_ImpulseDefinition.m_ImpulseDuration},力度{weaponData.impulseValue2}");
                        }
                        break;
                    }
                case 3:
                    {
                        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.durationValue3;
                        impulseSource.GenerateImpulse(weaponData.impulseValue3 * WeaponDirection);
                        if (weaponData.PrintHit)
                        {
                            Debug.Log($"当前的攻击类型是{weaponOwner.HitStrength},持续时间{impulseSource.m_ImpulseDefinition.m_ImpulseDuration},力度{weaponData.impulseValue3}");
                        }
                        break;
                    }
                case 4:
                    {
                        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.durationValue4;
                        impulseSource.GenerateImpulse(weaponData.impulseValue4 * WeaponDirection);
                        if (weaponData.PrintHit)
                        {
                            Debug.Log($"当前的攻击类型是{weaponOwner.HitStrength},持续时间{impulseSource.m_ImpulseDefinition.m_ImpulseDuration},力度{weaponData.impulseValue4}");
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
    /// 播放这个武器的攻击特效
    /// </summary>
    /// <param name="HitNum"></param>
    public void PlayHittedFx(int HitNum = 0)
    {
        Debug.Log("播放击中特效");
        //if (HittedFx[0]!=null)
        //{
        //    ParticleSystem particle = HittedFx[0];
        //    particle.transform.position = this.GetComponentInChildren<WeaponFx>().transform.position;
        //    HittedFx[0].Play(true);
        //}
    }
    /// <summary>
    /// 播放击中特效
    /// </summary>
    public void PlayFX()
    {

    }

    /// <summary>
    /// 一个时停加震动的复合方法。
    /// </summary>
    /// <param name="fadeInDuration"></param>
    /// <param name="fadeOutDuration"></param>
    /// <param name="duration"></param>
    /// <param name="targetTimeScale"></param>
    /// <param name="weaponManager"></param>
    /// <returns></returns>

    /// <summary>
    /// 相对于人物坐标系武器的运动方向
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
