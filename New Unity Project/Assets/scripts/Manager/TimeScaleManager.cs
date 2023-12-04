using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

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

    // 设置时间缩放
    public void SetTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
    }

    // 重置时间缩放为初始值
    public void ResetTimeScale()
    {
        Time.timeScale = originalTimeScale;
    }

    // 获取当前时间缩放值
    public float GetCurrentTimeScale()
    {
        return Time.timeScale;
    }

    // 暂停游戏
    public void PauseGame()
    {
        SetTimeScale(0f);
    }

    // 恢复游戏
    public void ResumeGame()
    {
        ResetTimeScale();
    }

    // 加速游戏时间
    public void SpeedUpGame(float factor)
    {
        SetTimeScale(originalTimeScale * factor);
    }

    // 减缓游戏时间
    public void SlowDownGame(float factor)
    {
        SetTimeScale(originalTimeScale / factor);
    }

    // 恢复正常游戏时间（取消加速或减缓效果）
    public void ResumeNormalTime()
    {
        ResetTimeScale();
    }



    public async UniTask SetTimeScale(float fadeInTime, float fadeOutTime, float duration, float targetTimeScale)
    {
        float currentTimeScale = Time.timeScale;

        // 渐入
        await DOTween.To(() => Time.timeScale, value => Time.timeScale = value, targetTimeScale, fadeInTime)
            .OnUpdate(() => { /* 可在更新时执行其他操作 */ })
            .AsyncWaitForCompletion();

        // 持续
        await UniTask.Delay((int)(duration * 1000));

        // 渐出
        await DOTween.To(() => Time.timeScale, value => Time.timeScale = value, originalTimeScale, fadeOutTime)
            .OnUpdate(() => { /* 可在更新时执行其他操作 */ })
            .AsyncWaitForCompletion();

        // 恢复到初始时间缩放
        ResetTimeScale();
    }


}
