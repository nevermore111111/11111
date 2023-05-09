using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public Animator animator;
    public List<PlayableAsset> timelines;

    private string currentAnimName;
    private PlayableDirector director;

    public AssetHelper myAssetHelper;
    PlayableAsset[] attackOnGround;
    PlayableAsset[] attackOnGroundFist;
    PlayableAsset[] attackInAir;
    

    private void Start()
    {
        // 加载配置文件
        LoadTimeLineAsset();
        currentAnimName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name; // 获取当前动画名称
        director = GetComponent<PlayableDirector>();
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
            attackInAir = myAssetHelper.AttackInAir;
        }
    }

    private void Update()
    {
        string animName = "";
        if (animator.GetCurrentAnimatorClipInfo(0).Length != 0)
        {
            animName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name; // 获取当前动画名称
        }
        if (animName != currentAnimName) // 当前动画名称和上一个不同
        {
            currentAnimName = animName; // 更新当前动画名称
            PlayTimelineByName(currentAnimName); // 播放对应名称的Playable
        }
    }

    private void PlayTimelineByName(string name)
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
        if (name == "AttackOnGround")
        {
            assets = attackOnGround;
        }
        else if (name == "AttackOnGround_fist")
        {
            assets = attackOnGroundFist;
        }
        else if (name == "AttackInAir")
        {
            assets = attackInAir;
        }

        if (assets != null)
        {
            timelines = assets.ToList();
        }
    }
}