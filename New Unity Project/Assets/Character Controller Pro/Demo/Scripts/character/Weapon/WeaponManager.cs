using Cinemachine;
using Cinemachine.Utility;
using Lightbug.CharacterControllerPro.Core;
using MagicaCloth2;
using Sirenix.OdinInspector;
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

    CinemachineBrain Brain;
    public WeaponKind kind;//这个武器的种类，会根据这个武器的种类去那应该有哪些探测器
    Detection[] detections;    //这个武器的所有探测器
    public WeaponDetector[] ActiveWeaponDetectors;//这个武器当前激活的探测器
    public bool isOnDetection;  //是否开启检测
    //CharacterActor characterActor;
    public bool isHited;
    private CinemachineImpulseSource impulseSource;
    Impulse impulse;
    public float[] impulsePar;
    [Tooltip("这里配置要装载哪一段特效")]
    public string[] weaponHitFx;
    public List<CharacterInfo> HittedCharacter;
    [Tooltip("这个配置CurrentAnimConfig.AttackDirection，配置相对于characteractor的local方向，在写入时已经转化成了世界坐标")]
    /// <summary>
    /// 这个配置CurrentAnimConfig.AttackDirection，配置相对于characteractor的local方向，在写入时已经转化成了世界坐标
    /// </summary>
    public Vector3 WeaponWorldDirection;
    public CharacterInfo weaponOwner;
    public WeaponData weaponData;
    public bool isNeedUpdateDirection = false;


    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        //characterActor = GetComponentInParent<CharacterActor>();
        weaponOwner = GetComponentInParent<CharacterInfo>();
        weaponData = FindFirstObjectByType<WeaponData>();
        impulse = GetComponent<Impulse>();
        Brain = FindObjectOfType<CinemachineBrain>();
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

                    还有firstordefaut
                    first等方法。
                    比如选择最近的，先orderby，然后first，直接可以选到最近的
                    select ， where会选到多个
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



    public void Update()
    {
        HandleDetection();
        //UpdateWeaponDirection();
        //shake();
        // Debug.Log(Time.timeScale);
    }



    /// <summary>
    /// 检测碰撞，如果在检测
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
                AttackReceive attack;
                foreach (var hit in item.GetDetection(out item.isHited))//添加了攻击对象
                {

                    if (hit.TryGetComponent(out attack) && attack.isNormalReceive())
                    {
                        AgetHitBox hitted = attack.CharacterInfo.hitBox;
                        hitted.GetWeapon(this);
                    }
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
            foreach (Detection item in detections)
            {
                item.ResetBeForeHit();
            }
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
        impulseSource.GenerateImpulse(impulseRank * WeaponWorldDirection);
    }


    /// <summary>
    /// 产生震动
    /// </summary>
    public void SPImpluse(string attackName)
    {
        switch (attackName)
        {
            case "sp11":
                {
                    impulseSource.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
                    impulseSource.m_ImpulseDefinition.m_ImpulseDuration = weaponData.sp11Duration;
                    impulseSource.GenerateImpulse(WeaponWorldDirection * weaponData.sp11Force);
                    break;
                }
        }
    }
    Vector3 targetShakeDirection;//每次震动时使用的中间变量

    [Button("震动")]
    /// <summary>
    /// 产生震动
    /// </summary>
    public void Impluse()
    {
        Debug.Log("暂时不播放震动");  
        return;
        if (weaponOwner is MainCharacter)
        {
            WeaponNum weaponNum = new WeaponNum();
            //设置当前的攻击数据
            weaponNum = SetCurrentWeaponNum(weaponNum);
            Vector3 targetShakeDirection;//每次震动时使用的中间变量
            ShakeChange(weaponNum,out targetShakeDirection);

            if (weaponData.isUseDotweenShake)
            {
                //使用dotween的shake
                CameraShakeManager.Instance.Shake(targetShakeDirection, weaponNum.Strength, weaponNum.Frequence, weaponNum.Duration);
            }
            else
            {
                //使用cinemachine的shake

                impulse.GenerateImpulse(targetShakeDirection, weaponNum.Strength, weaponNum.Frequence, weaponNum.Duration);
            }


        }
        else
        {

        }
    }
    /// <summary>
    /// 根据选项，转换当前的震动信号，比如忽略z等
    /// </summary>
    private void ShakeChange(WeaponNum weaponNum, out Vector3 targetShakeDirection)
    {
        targetShakeDirection = Vector3.zero;
        if (Brain == null)
        {
            Debug.LogError("没找到cinemachineBrain");
            return;
        }
        if (weaponData.isIgnoreZshake)
        {
            //修改震动.//忽略在主摄像机z轴的震动
            Debug.Log("当前忽略的z方向的震动");
            targetShakeDirection = WeaponWorldDirection.ProjectOntoPlane(Brain.transform.forward).normalized;
            //Debug.DrawLine(Brain.transform.position, Brain.transform.position + targetShakeDirection,Color.red,1f);
        }
        if (weaponData.onlyUseVirticalShake)
        {
            Debug.Log("当前只使用竖直方向的震动");
            targetShakeDirection = Vector3.Project(WeaponWorldDirection,Brain.transform.up).normalized;
            //Debug.DrawLine(Brain.transform.position, Brain.transform.position + targetShakeDirection, Color.blue, 1f);
        }
        if (weaponData.PrintHit)
        {
            Debug.Log($"攻击力度：{weaponOwner.HitStrength}，震动力度{weaponNum.Strength}，震动频率{weaponNum.Frequence}，震动时间{weaponNum.Duration}");
        }
        Debug.DrawLine(Brain.transform.position, Brain.transform.position+targetShakeDirection);

    }

    /// <summary>
    /// 根据打击力度设置当前的weaponNum（影响震动）
    /// </summary>
    /// <param name="weaponNum"></param>
    /// <returns></returns>
    private WeaponNum SetCurrentWeaponNum(WeaponNum weaponNum)
    {
        if (weaponData.weaponNumList.Count >= weaponOwner.HitStrength + 1 && weaponData.weaponNumList[weaponOwner.HitStrength] != null)
        {
            weaponNum = weaponData.weaponNumList[weaponOwner.HitStrength];
        }
        else
        {
            Debug.LogError($"weaponNum索引[{weaponOwner.HitStrength}]缺少数据");
        }

        return weaponNum;
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
        return HittedCharacterTransform.InverseTransformDirection(WeaponWorldDirection);
    }
}
public enum WeaponKind
{
    nullArm,
    sword,
    fist
}
