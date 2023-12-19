using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class TimeScaleManager : MonoBehaviour
{
    private static TimeScaleManager _instance;

    public static TimeScaleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TimeScaleManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("TimeScaleManager");
                    _instance = singletonObject.AddComponent<TimeScaleManager>();
                }
            }

            return _instance;
        }
    }

    private float originalTimeScale = 1f;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Save the original time scale at the start
        originalTimeScale = Time.timeScale;
    }

    // ����ʱ������
    public void SetTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
    }
    //������õĵ�ǰʱ���������ȼ����ڵ��ڣ��ͻ���Ч
    public void SetTimeScaleByPriority(float newTimeScale, int priority)
    {
        if (priority >= currentPriority)
        {
            currentPriority = priority;
            Time.timeScale = newTimeScale;
        }
    }

    // ����ʱ������Ϊ��ʼֵ
    public void ResetTimeScale()
    {
        Time.timeScale = originalTimeScale;
    }

    // ��ȡ��ǰʱ������ֵ
    public float GetCurrentTimeScale()
    {
        return Time.timeScale;
    }

    // ��ͣ��Ϸ
    public void PauseGame()
    {
        SetTimeScale(0f);
    }

    // �ָ���Ϸ
    public void ResumeGame()
    {
        ResetTimeScale();
    }

    // ������Ϸʱ��
    public void SpeedUpGame(float factor)
    {
        SetTimeScale(originalTimeScale * factor);
    }

    // ������Ϸʱ��
    public void SlowDownGame(float factor)
    {
        SetTimeScale(originalTimeScale / factor);
    }

    // �ָ�������Ϸʱ�䣨ȡ�����ٻ����Ч����
    public void ResumeNormalTime()
    {
        ResetTimeScale();
    }

    private int currentPriority;

    /// <summary>
    /// ���������ʱ���Ϊ����ʱ�����ŵ�
    /// </summary>
    /// <param name="fadeInTime"></param>
    /// <param name="fadeOutTime"></param>
    /// <param name="duration"></param>
    /// <param name="targetTimeScale"></param>
    /// <param name="changePriority"></param>
    /// <returns></returns>
    public async UniTask SetTimeScale(float fadeInTime, float fadeOutTime, float duration, float targetTimeScale, int changePriority = 0)
    {
        float currentTimeScale = Time.timeScale;

        // ����
        await DOTween.To(() => Time.timeScale, value => SetTimeScaleByPriority(value, changePriority), targetTimeScale, fadeInTime)
            .OnUpdate(() => { /* ���ڸ���ʱִ���������� */ }).SetUpdate(true)
            .AsyncWaitForCompletion();
        // ����
        await UniTask.Delay((int)(duration * 1000),true);

        // ����
        await DOTween.To(() => Time.timeScale, value => SetTimeScaleByPriority(value, changePriority), originalTimeScale, fadeOutTime)
            .OnUpdate(() => { /* ���ڸ���ʱִ���������� */ }).SetUpdate(true)
            .AsyncWaitForCompletion();

        // �ָ�����ʼʱ������
        ResetTimeScale();
        currentPriority = 0;
    }



    //public async UniTask SetTimeScale(float fadeInTime, float fadeOutTime, float duration, float targetTimeScale)
    //{
    //    float currentTimeScale = Time.timeScale;

    //    // ����
    //    await DOTween.To(() => Time.timeScale, value => Time.timeScale = value, targetTimeScale, fadeInTime)
    //        .OnUpdate(() => { /* ���ڸ���ʱִ���������� */ })
    //        .AsyncWaitForCompletion();

    //    // ����
    //    await UniTask.Delay((int)(duration * 1000));

    //    // ����
    //    await DOTween.To(() => Time.timeScale, value => Time.timeScale = value, originalTimeScale, fadeOutTime)
    //        .OnUpdate(() => { /* ���ڸ���ʱִ���������� */ })
    //        .AsyncWaitForCompletion();

    //    // �ָ�����ʼʱ������
    //    ResetTimeScale();
    //}
}
