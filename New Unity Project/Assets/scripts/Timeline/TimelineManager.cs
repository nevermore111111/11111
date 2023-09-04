using Rusk;
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
    PlayableAsset[] attackOffGround;
    PlayableAsset[] startPlay;
    PlayableAsset[] normalMovement;
    PlayableAsset[] evade;




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
            attackOffGround = myAssetHelper.AttackOffGround;
            evade = myAssetHelper.Evade;
            startPlay = myAssetHelper.StartPlay;
            normalMovement = myAssetHelper.NormalMovement;
        }
    }

    

    public void PlayTimelineByName(string name)
    {
        foreach (PlayableAsset playable in timelines)
        {
            if (playable.name == name) // �ҵ�����ƥ���Playable
            {
                director.playableAsset = playable; // ����Playable
                director.Play(); // ����Playable
                return;
            }
        }
        Debug.Log("û�ҵ���Ӧ��timeline");
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
            case "StartPlay":
                assets = startPlay;
                break;
        }
        if (assets != null)
        {
            timelines = assets.ToList();
        }
    }
}