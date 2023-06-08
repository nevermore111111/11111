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
    /// ���ݼ����������fov
    /// </summary>
    private void ChangeZoom()
    {
        // ��ȡ��������
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");

        // �����ӽ�Զ��
        freeLookCamera.m_Lens.FieldOfView += zoomInput * zoomSpeed;

        // �����ӽǷ�Χ
        freeLookCamera.m_Lens.FieldOfView = Mathf.Clamp(freeLookCamera.m_Lens.FieldOfView, 40f, 60f);
    }

    /// <summary>
    /// �仯���𶯷��ȣ��𽥽���
    /// </summary>
    private void Shake()
    {
        if (shakeElapsedTime > 0)
        {
            float normalizedShakeTime = Mathf.Clamp01(shakeElapsedTime / shakeDuration); // �����һ������ʱ��
            float currentShakeAmplitude = shakeAmplitude * normalizedShakeTime; // ���ݹ�һ����ʱ����㵱ǰ���𶯷���

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
        shakeElapsedTime = shakeDuration; // �����𶯳���ʱ��
    }

    /// <summary>
    /// ������𶯷���
    /// </summary>
    /// <param name="shakeTime"></param>
    /// <param name="amplitude"></param>
    public void ShakeCamera(float shakeTime, float amplitude)
    {
        shakeDuration = shakeTime; // �����𶯳���ʱ��
        shakeElapsedTime = shakeTime; // ��ʼ��

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
    /// �仯����������������Զ���
    /// </summary>
    /// <param name="shakeTime"></param>
    /// <param name="amplitude"></param>
    /// <param name="frequency"></param>
    public void ShakeCamera(float shakeTime, float amplitude,float frequency)
    {
        shakeDuration = shakeTime; // �����𶯳���ʱ��
        shakeElapsedTime = shakeTime; // ��ʼ��

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
