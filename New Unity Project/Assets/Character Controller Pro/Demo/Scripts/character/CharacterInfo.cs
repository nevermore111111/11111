using Cysharp.Threading.Tasks;
using DG.Tweening;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using static HitTimeData;

[RequireComponent(typeof(AgetHitBox))]
public abstract class CharacterInfo : MonoBehaviour, IAgent
{
    private static int uniqueID = 1;
    public int Id { get; private set; }

    //周围全部的敌人
    public CharacterActor characterActor;
    public CharacterStateController CharacterStateController { get; private set; }


    [Tooltip("代表这个单位的敌人tag")]
    public string enemyTag;
    [Tooltip("代表这个单位的敌人列表")]
    public List<CharacterInfo> enemies;
    [Tooltip("这个单位的选择目标")]
    public CharacterInfo selectEnemy;
    [Tooltip("这个人物在摄像机中的大小半径，cinemachine的TargetGrounp中的size")]
    public float cameraRadius = 0.5f;

    public AgetHitBox hitBox;
    public int HitStrength;
    public bool canBeExecuted = false;

    [HideInInspector]
    public List<AttackReceive> allReceives;
    [HideInInspector]
    public List<SkillReceiver> allSkillReceivers;

    public AttackAndDefendInfo attackAndDefendInfo = new AttackAndDefendInfo();

    private Hitted Hitted;

    //判断是否去闪避，如果update里判定true，就去闪避
    public bool ToEvade
    {
        get
        {
            return toEvade;
        }
        set
        {
            toEvade = value;
            if(value)//进行闪避时
            {
                UniTask.Delay(150).ContinueWith(() =>
                {
                    if (Time.time - lastTimeToEvade > 0.12f/*大于0.12就认为是一次*/)
                    {
                        toEvade = false;
                    };
                });
                lastTimeToEvade = Time.time;
            }
        }
    }
    private float lastTimeToEvade;
    private bool toEvade = false;

    /// <summary>
    /// 伤害，目标位置，武器方向，击中类型
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="pos"></param>
    /// <param name="weapon">这个是别人的武器</param>
    /// <param name="collider">受击位置</param>
    /// <param name="hit"></param>
    virtual public void GetDamage(float damage, Vector3 pos, WeaponManager weapon, Collider collider, IAgent.HitKind hit = IAgent.HitKind.ground)
    {
        if (Hitted != null)
            Hitted.GetHitted(weapon, hit);
        attackAndDefendInfo.OnHit?.Invoke();
        FxManagerPro.Instance.PlayFx(weapon.weaponHitFx, collider.transform);
    }
    virtual public void GetDamage(float damage, Vector3 attackDirection, float hitStrength, string targetAnimToPlay = null)
    {
        Debug.Log("这个方法没写完，现在只会播放动作");
        if (!targetAnimToPlay.IsNullOrEmpty())
        {
            characterActor.Animator.CrossFadeInFixedTime(targetAnimToPlay, 0.1f);
        }
    }

    public SkillReceiver GetSkillReceiver(int requireSkillPointNum)
    {
        return allSkillReceivers.FirstOrDefault(_ => _.skillPoint == requireSkillPointNum);
    }

    protected virtual void Awake()
    {
        enemies = new List<CharacterInfo>();
        hitBox = GetComponentInChildren<AgetHitBox>();
        characterActor = GetComponentInParent<CharacterActor>();
        CharacterStateController = this.transform.parent?.GetComponentInChildren<CharacterStateController>();
        attackAndDefendInfo.weaponManagers = GetComponentsInChildren<WeaponManager>();
        SetUniqueID();
    }



    protected virtual void Start()
    {
        Hitted = characterActor?.stateController.GetState<Hitted>() as Hitted;
        hitTimeData = FindObjectOfType<HitTimeData>();
    }
    public HitTimeData hitTimeData;
    virtual public void HitOther(WeaponManager weaponManager)
    {
        HitParByHitKind(weaponManager);
    }
    //这个是我打到别人的方法
    public void HitParByHitKind(WeaponManager weapon)
    {

#if UNITY_EDITOR
        // 只在Unity编辑器中运行的代码
        if (hitTimeData.ForceCurrent > -1)
        {
            //如果强制使用这个震动，会播放这个震动的震屏效果
            HitStrength = hitTimeData.ForceCurrent;
        }
#endif
        HitPlus(HitStrength, hitTimeData, weapon);
    }
    public async void HitPlus(int currentHit, HitTimeData hitTimeData, WeaponManager weaponManager)
    {
        hitTimeData.CurrentHit = currentHit;
        float fadeInDuration = hitTimeData.currentHitTimePara.fadeTime;//hitData.GetFadeTime(hitData, currentHit);
        float fadeOutDuration = hitTimeData.currentHitTimePara.fadeTime / 2f;// 渐出时间稍微短一些
        float duration = hitTimeData.currentHitTimePara.stayTime;
        float targetTimeScale = hitTimeData.currentHitTimePara.targetTimeScale;
        //TimeScaleManager.Instance.SetTimeScale(fadeInDuration, fadeOutDuration, duration, targetTimeScale);
        //减速自身和目标的速度。
        if (currentHit == 3)
            await TimeScaleManager.Instance.SetAnimatorSpeed(fadeInDuration, fadeOutDuration, duration, targetTimeScale, new List<Animator> { characterActor.Animator, selectEnemy?.characterActor?.Animator });
        if (currentHit == 3)
            if (weaponManager != null && weaponManager.isActiveAndEnabled)
            {
                //调用震动和特效
                weaponManager.Impluse();
            }
    }

    //_______________________________________________________子方法分隔线_______________________________________________________________
    //_______________________________________________________子方法分隔线_______________________________________________________________
    //_______________________________________________________子方法分隔线_______________________________________________________________
    /// <summary>
    /// 设置每个characterInfo实例都有一个唯一ID。
    /// </summary>
    private void SetUniqueID()
    {
        Id = uniqueID;
        uniqueID++;
    }

}
