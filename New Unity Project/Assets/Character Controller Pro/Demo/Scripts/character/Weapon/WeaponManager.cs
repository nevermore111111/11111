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

  
    public WeaponKind kind;
    Detection[] detections;
    //是否开启检测
    public bool isOnDetection;
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
    public Detection[] Detections;
    public WeaponDetector[] ActiveWeaponDetectors;

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
            if(detections[i].Weapon == null)
            detections[i].Weapon = this;
        }
        Detection detection = GetComponentInChildren<Detection>();
        Detections = GetComponentsInChildren<Detection>();
        /*SetColliderByDetect(detection);*///根据这个检测去生成一个碰撞盒
    }

    //private static void SetColliderByDetect(Detection detection)
    //{
    //    if (detection is CapsuleDetection)
    //    {
    //        CapsuleDetection detectionCas = detection as CapsuleDetection;
    //        GameObject colliderObject = new GameObject("collider");
    //        CapsuleCollider capsuleCollider = colliderObject.AddComponent<CapsuleCollider>();

    //        // 可以在这里设置 CapsuleCollider 的属性
    //        capsuleCollider.center = (detectionCas.startPoint.position + detectionCas.endPoint.position) / 2;
    //        capsuleCollider.radius =  detectionCas.radius;
    //        capsuleCollider.height = (detectionCas.startPoint.position - detectionCas.endPoint.position).magnitude+2*detectionCas.radius;
    //    }
    //    else if (true)
    //    {

    //    }
    //}

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
        //shake();
        // Debug.Log(Time.timeScale);
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
                if(!this.ActiveWeaponDetectors.Contains(item.WeaponDetector))
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
    /// 产生震动
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
    /// 播放这个武器的攻击特效
    /// </summary>
    /// <param name="HitNum"></param>
    public void PlayHittedFx(int HitNum = 0)
    {
        ParticleSystem particle = HittedFx[0];
        particle.transform.position = this.GetComponentInChildren<WeaponFx>().transform.position;
        HittedFx[0].Play(true);
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
