using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public Animator animator;
    public PlayableAsset[] timelines;

    private string currentAnimName;
    private PlayableDirector director;

    private void Start()
    {
        currentAnimName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name; // 获取当前动画名称
        director = GetComponent<PlayableDirector>();
        PlayTimelineByName(currentAnimName); // 播放对应名称的Playable
    }

    private void Update()
    {
        string animName  = "";
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
}