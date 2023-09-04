using Rusk;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 这个方法去更新每个攻击的特效和镜头参数
/// </summary>
public class TimelineManager : MonoBehaviour
{
    public Animator animator;
    public List<PlayableAsset> timelines;

    private string currentAnimName;
    private PlayableDirector director;

    public AssetHelper myAssetHelper;
    PlayableAsset[] attackOnGround;
    PlayableAsset[] attackOnGroundFist;
    PlayableAsset[] attackOffGround;
    PlayableAsset[] startPlay;
    PlayableAsset[] normalMovement;
    PlayableAsset[] evade;




    private void Start()
    {
        // 加载配置文件
        director = GetComponent<PlayableDirector>();
        LoadTimeLineAsset();
        currentAnimName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name; // 获取当前动画名称
        PlayTimelineByName(currentAnimName); // 播放对应名称的Playable
    }

    /// <summary>
    /// 加载timeline的文件
    /// </summary>
    private void LoadTimeLineAsset()
    {
        myAssetHelper = Resources.Load<AssetHelper>("AssetHelper");
        {
            attackOnGround = myAssetHelper.AttackOnGround;
            attackOnGroundFist = myAssetHelper.AttackOnGround_fist;
            attackOffGround = myAssetHelper.AttackOffGround;
            evade = myAssetHelper.Evade;
            startPlay = myAssetHelper.StartPlay;
            normalMovement = myAssetHelper.NormalMovement;
        }
    }

    //private void Update()
    //{
    //    string animName = "";
    //    if (animator.GetCurrentAnimatorClipInfo(0).Length != 0)
    //    {
    //        animName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name; // 获取当前动画名称
    //    }
    //    if (animName != currentAnimName) // 当前动画名称和上一个不同
    //    {
    //        currentAnimName = animName; // 更新当前动画名称
    //        PlayTimelineByName(currentAnimName); // 播放对应名称的Playable
    //    }
    //}

    public void PlayTimelineByName(string name)
    {
        foreach (PlayableAsset playable in timelines)
        {
            if (playable.name == name) // 找到名称匹配的Playable
            {
                director.playableAsset = playable; // 设置Playable
                director.Play(); // 播放Playable
                break;
            }
        }
    }
    public void SwapTimelinesByAssetName(string name)
    {
        PlayableAsset[] assets = null;
        switch (name) 
        {
            case "attackOnGround":
                assets = attackOnGround;
                break;
            case "AttackOnGround_fist":
                assets = attackOnGroundFist;
                break;
            case "AttackOffGround":
                assets = attackOffGround;
                break;
            case "NormalMovement":
                assets = normalMovement;
                break;
            case "Evade":
                assets = evade;
                break;
        }
        if (assets != null)
        {
            timelines = assets.ToList();
        }
    }
}