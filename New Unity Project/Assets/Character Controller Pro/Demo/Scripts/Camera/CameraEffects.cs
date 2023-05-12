using UnityEngine;
using Cinemachine;

public class CameraEffects : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public float shakeDuration = 0.3f;
    public float shakeAmplitude = 1.2f;
    public float shakeFrequency = 2.0f;

    private float shakeElapsedTime = 0f;
    private CinemachineBasicMultiChannelPerlin noise;

    void Start()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        if (freeLookCamera != null)
            noise = freeLookCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Debug.Log(freeLookCamera.GetRig(1));
    }

    void Update()
    {
        if (shakeElapsedTime > 0)
        {
            noise.m_AmplitudeGain = shakeAmplitude;
            noise.m_FrequencyGain = shakeFrequency;
            shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            noise.m_AmplitudeGain = 0f;
            shakeElapsedTime = 0f;
        }
        if(Input.GetKeyUp(KeyCode.H))
        {
            ShakeCamera();
            Debug.Log("»Î¶¯");
        }
    }

    public void ShakeCamera()
    {
        shakeElapsedTime = shakeDuration;
    }
}
