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
        currentAnimName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name; // ��ȡ��ǰ��������
        director = GetComponent<PlayableDirector>();
        PlayTimelineByName(currentAnimName); // ���Ŷ�Ӧ���Ƶ�Playable
    }

    private void Update()
    {
        string animName  = "";
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
}