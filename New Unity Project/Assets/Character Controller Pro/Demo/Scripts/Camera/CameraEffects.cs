using UnityEngine;
using Cinemachine;

public class CameraEffects : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public float shakeDuration = 0.3f;
    public float shakeAmplitude = 1.2f;
    public float shakeFrequency = 2.0f;

    private float shakeElapsedTime = 0f;
    private CinemachineBasicMultiChannelPerlin[] noises;

    public float zoomSpeed = 10f;

    public MainCharacter MainCharacter;

    void Start()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        if (freeLookCamera != null)
        {
            noises = new CinemachineBasicMultiChannelPerlin[freeLookCamera.m_Orbits.Length];
            for (int i = 0; i < freeLookCamera.m_Orbits.Length; i++)
            {
                noises[i] = freeLookCamera.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
        }
    }

    void Update()
    {
        Shake();
        if (Input.GetKeyDown(KeyCode.H))
        {
            ShakeCamera();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            ShakeCamera(shakeDuration, shakeAmplitude);
        }

        ChangeZoom();
    }

    private void SetCamera()
    {
        if (MainCharacter.enemys.Count == 0)
        {
            freeLookCamera.Priority = 100;
        }
        else
        {
            freeLookCamera.Priority = 5;
        }
    }    


    /// <summary>
    /// 根据键盘输入调整fov
    /// </summary>
    private void ChangeZoom()
    {
        // 获取键盘输入
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");

        // 调整视角远近
        freeLookCamera.m_Lens.FieldOfView += zoomInput * zoomSpeed;

        // 限制视角范围
        freeLookCamera.m_Lens.FieldOfView = Mathf.Clamp(freeLookCamera.m_Lens.FieldOfView, 40f, 60f);
    }

    /// <summary>
    /// 变化的震动幅度，逐渐降低
    /// </summary>
    private void Shake()
    {
        if (shakeElapsedTime > 0)
        {
            float normalizedShakeTime = Mathf.Clamp01(shakeElapsedTime / shakeDuration); // 计算归一化的震动时间
            float currentShakeAmplitude = shakeAmplitude * normalizedShakeTime; // 根据归一化的时间计算当前的震动幅度

            foreach (CinemachineBasicMultiChannelPerlin noise in noises)
            {
                if (noise)
                {
                    noise.m_AmplitudeGain = currentShakeAmplitude;
                    noise.m_FrequencyGain = shakeFrequency;
                    
                }
            }

            shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            foreach (CinemachineBasicMultiChannelPerlin noise in noises)
            {
                if (noise)
                {
                    noise.m_AmplitudeGain = 0f;
                }
            }

            shakeElapsedTime = 0f;
        }
    }

    public void ShakeCamera()
    {
        shakeElapsedTime = shakeDuration; // 设置震动持续时间
    }

    /// <summary>
    /// 不变的震动幅度
    /// </summary>
    /// <param name="shakeTime"></param>
    /// <param name="amplitude"></param>
    public void ShakeCamera(float shakeTime, float amplitude)
    {
        shakeDuration = shakeTime; // 设置震动持续时间
        shakeElapsedTime = shakeTime; // 开始震动

        foreach (CinemachineBasicMultiChannelPerlin noise in noises)
        {
            if (noise)
            {
                noise.m_AmplitudeGain = amplitude;
                noise.m_FrequencyGain = shakeFrequency;
            }
        }
    }
    /// <summary>
    /// 变化的振幅，三个参数自定义
    /// </summary>
    /// <param name="shakeTime"></param>
    /// <param name="amplitude"></param>
    /// <param name="frequency"></param>
    public void ShakeCamera(float shakeTime, float amplitude,float frequency)
    {
        shakeDuration = shakeTime; // 设置震动持续时间
        shakeElapsedTime = shakeTime; // 开始震动

        foreach (CinemachineBasicMultiChannelPerlin noise in noises)
        {
            if (noise)
            {
                noise.m_AmplitudeGain = amplitude;
                noise.m_FrequencyGain = frequency;
            }
        }
    }
}
