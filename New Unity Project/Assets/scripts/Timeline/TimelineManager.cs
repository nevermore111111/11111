using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Implementation;
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
    private List<PlayableAsset> timelines;
    private PlayableDirector director;
    public AssetHelper myAssetHelper;


    private void Start()
    {
        // 加载配置文件
        director = GetComponent<PlayableDirector>();
        LoadTimeLineAsset();
    }

    /// <summary>
    /// 加载timeline的文件
    /// </summary>
    private void LoadTimeLineAsset()
    {
        myAssetHelper = Resources.Load<AssetHelper>("AssetHelper");
        timelines = myAssetHelper.All.ToList();
    }

    

    public void PlayTimelineByName(string name)
    {
        foreach (PlayableAsset playable in timelines)
        {
            if (playable?.name == name) // 找到名称匹配的Playable
            {
                director.playableAsset = playable; // 设置Playable
                director.Play(); // 播放Playable
                return;
            }
        }
        Debug.Log("没找到对应的timeline");
    }
  
}