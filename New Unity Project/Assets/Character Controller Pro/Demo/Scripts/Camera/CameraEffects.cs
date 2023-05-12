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
        if (shakeElapsedTime > 0)
        {
            foreach (CinemachineBasicMultiChannelPerlin noise in noises)
            {
                if (noise)
                {
                    noise.m_AmplitudeGain = shakeAmplitude;
                    noise.m_FrequencyGain = shakeFrequency;
                }
            }
            shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            foreach (CinemachineBasicMultiChannelPerlin noise in noises)
            {
                if(noise)
                noise.m_AmplitudeGain = 0f;
            }
            shakeElapsedTime = 0f;
        }

        if (Input.GetKeyUp(KeyCode.H))
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
