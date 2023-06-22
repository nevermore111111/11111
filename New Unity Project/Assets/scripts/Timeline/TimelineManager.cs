using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// �������ȥ����ÿ����������Ч�;�ͷ����
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
    PlayableAsset[] attackInAir;
    PlayableAsset[] start;
    


    private void Start()
    {
        // ���������ļ�
        director = GetComponent<PlayableDirector>();
        LoadTimeLineAsset();
        currentAnimName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name; // ��ȡ��ǰ��������
        PlayTimelineByName(currentAnimName); // ���Ŷ�Ӧ���Ƶ�Playable
    }

    /// <summary>
    /// ����timeline���ļ�
    /// </summary>
    private void LoadTimeLineAsset()
    {
        myAssetHelper = Resources.Load<AssetHelper>("AssetHelper");
        {
            attackOnGround = myAssetHelper.AttackOnGround;
            attackOnGroundFist = myAssetHelper.AttackOnGround_fist;
            attackInAir = myAssetHelper.AttackInAir;
            start = myAssetHelper.Start;
        }
        director.Play(start[0]);
    }

    private void Update()
    {
        string animName = "";
        if (animator.GetCurrentAnimatorClipInfo(0).Length != 0)
        {
            animName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name; // ��ȡ��ǰ��������
        }
        if (animName != currentAnimName) // ��ǰ�������ƺ���һ����ͬ
        {
            currentAnimName = animName; // ���µ�ǰ��������
            PlayTimelineByName(currentAnimName); // ���Ŷ�Ӧ���Ƶ�Playable
        }
    }

    private void PlayTimelineByName(string name)
    {
        foreach (PlayableAsset playable in timelines)
        {
            if (playable.name == name) // �ҵ�����ƥ���Playable
            {
                director.playableAsset = playable; // ����Playable
                director.Play(); // ����Playable
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