using Cinemachine;
using Lightbug.CharacterControllerPro.Core;
using MagicaCloth2;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

//我需要做的一个功能 ，在攻击hit事件的时候判定当前的武器是否集中了敌人，如果集中了敌人，那么就震动摄像机，而且将我和目标的动画播放速度降低
[RequireComponent(typeof(CinemachineImpulseSource))]
[RequireComponent(typeof(Detection))]
public class WeaponManager : MonoBehaviour
{
    
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
    public void UpdateWeaponDirection()
    {
        if (frameCount == 0)
        {
            // 获取当前武器位置
            Vector3 currentWeaponPosition = transform.position;

            if (frameCount == 3)
            {
                // 计算武器方向
                WeaponDirection = currentWeaponPosition - previousWeaponPosition;
                WeaponDirection.Normalize();
            }

            // 更新上一帧的武器位置
            previousWeaponPosition = currentWeaponPosition;
        }

        // 增加帧计数
        frameCount = (frameCount + 1) % 3;
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
        
        if(isOnDetection)
        {
            foreach(Detection item in detections)
            {
                foreach (var hit in item.GetDetection(out item.isHited))//添加了攻击对象
                {
                    
                    AgetHitBox hitted = hit.GetComponent<AgetHitBox>();
                    //hitted.GetDamage(1, transform.position);//这是攻击对象播放都动画
                    hitted.GetWeapon(this);
                }
                //如果存在当前的detection击中目标，那么将武器是否击中目标也改成true。
                if(item.isHited == true)
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
            foreach(var item in detections)
            {
                item.ClaerWasHit();
                //清空hit列表，所有是否击中也全部清空
                
            }
            isHited=false;//武器击中判定也清空
        }
    }

    /// <summary>
    /// 产生震动
    /// </summary>
    public void Impluse(int i = 0)
    {
        if (impulsePar.Length >3 && impulsePar[0]==1)
        {
            impulseSource.GenerateImpulse(WeaponDirection);
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
    /// 需要做一个关于武器的方法，只在第一次击中一个characterINFO时才调用，需要传入的是击中的判定区域，击中的collider
    /// </summary>
    public void Hitted()
    {
        Debug.Log("击中");
        StartCoroutine(AdjustTimeScaleOverDuration(0.03f, 0.05f, 1f, 0.2f, this));
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
    public System.Collections.IEnumerator AdjustTimeScaleOverDuration(float fadeInDuration, float fadeOutDuration, float duration, float targetTimeScale, WeaponManager weaponManager)
    {
        float initialTimeScale = Time.timeScale;
        float elapsedTime = 0f;

        // 渐入
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / fadeInDuration);
            Time.timeScale = Mathf.Lerp(initialTimeScale, targetTimeScale, normalizedTime);
            // 可以在这里根据需要进行其他的逻辑处理

            // 等待一帧
            yield return null;
        }

        // 设置目标时间缩放
        Time.timeScale = targetTimeScale;
        weaponManager.PlayHittedFx();
        // 持续时间
        yield return new WaitForSecondsRealtime(duration);

        // 渐出
        elapsedTime = 0f;

        //调用震动和特效
        weaponManager.Impluse();
        //这里需要调用两个地方产生特效，一个是自身的刀光额外特效，另外一个是怪物的受击反馈。
        //需要做个委托


        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / fadeOutDuration);
            Time.timeScale = Mathf.Lerp(targetTimeScale, 1f, normalizedTime);
            // 可以在这里根据需要进行其他的逻辑处理

            // 等待一帧
            yield return null;
        }
        //一般在时停的最后时间再去调用摄像机的震动效果。
        // 恢复原始的时间缩放
        Time.timeScale = 1f;
    }
    /// <summary>
    /// 相对于人物坐标系武器的运动方向
    /// </summary>
    /// <returns></returns>
    public Vector3 GetWeaponDirectInverse(Transform HittedCharacterTransform)
    {
        return HittedCharacterTransform.InverseTransformDirection(WeaponDirection);
    }
}
