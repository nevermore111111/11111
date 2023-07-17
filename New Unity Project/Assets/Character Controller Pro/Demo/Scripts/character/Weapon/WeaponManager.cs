using Cinemachine;
using Lightbug.CharacterControllerPro.Core;
using MagicaCloth2;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

//����Ҫ����һ������ ���ڹ���hit�¼���ʱ���ж���ǰ�������Ƿ����˵��ˣ���������˵��ˣ���ô��������������ҽ��Һ�Ŀ��Ķ��������ٶȽ���
[RequireComponent(typeof(CinemachineImpulseSource))]
[RequireComponent(typeof(Detection))]
public class WeaponManager : MonoBehaviour
{
    
    Detection[] detections;
    //�Ƿ������
    public bool isOnDetection;
    CharacterActor characterActor;
    public bool isHited;
    private CinemachineImpulseSource impulseSource;
    public float[] impulsePar;
    [Tooltip("��������Ҫװ����һ����Ч")]
    public int FxLoad;

    public ParticleSystem[] HittedFx;

    public List<CharacterInfo> HittedCharacter;
    public Vector3 WeaponDirection;
    private int frameCount = 0;
    private Vector3 previousWeaponPosition;


    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        detections = GetComponents<Detection>();
        characterActor= GetComponentInParent<CharacterActor>();
        switch(FxLoad)
        {
            case 1: HittedFx = Resources.Load<FxHelper>("FxHelper").Sword;break;
                case 2: break;
        }
        for(int i = 0; i <detections.Length; i++)
        {
            detections[i].WeaponManagerOwner = this;
        }
        
    }
    public void UpdateWeaponDirection()
    {
        if (frameCount == 0)
        {
            // ��ȡ��ǰ����λ��
            Vector3 currentWeaponPosition = transform.position;

            if (frameCount == 3)
            {
                // ������������
                WeaponDirection = currentWeaponPosition - previousWeaponPosition;
                WeaponDirection.Normalize();
            }

            // ������һ֡������λ��
            previousWeaponPosition = currentWeaponPosition;
        }

        // ����֡����
        frameCount = (frameCount + 1) % 3;
    }
    public void Update()
    {
        HandleDetection();
        UpdateWeaponDirection();
        //shake();
        // Debug.Log(Time.timeScale);
    }

    /// <summary>
    /// ��⣬����ڼ�⣬��
    /// </summary>
    void HandleDetection()
    {
        
        if(isOnDetection)
        {
            foreach(Detection item in detections)
            {
                foreach (var hit in item.GetDetection(out item.isHited))//����˹�������
                {
                    
                    AgetHitBox hitted = hit.GetComponent<AgetHitBox>();
                    //hitted.GetDamage(1, transform.position);//���ǹ������󲥷Ŷ�����
                    hitted.GetWeapon(this);
                }
                //������ڵ�ǰ��detection����Ŀ�꣬��ô�������Ƿ����Ŀ��Ҳ�ĳ�true��
                if(item.isHited == true)
                {
                    //Impluse();//������������Ļ�������ķ�����
                    isHited = true;
                }
            }
        }
    }
    /// <summary>
    /// �رռ��ķ���
    /// </summary>
    /// <param name="value"></param>
    public void ToggleDetection(bool value)
    {
        isOnDetection = value;
        if (isOnDetection)
        {
            HandleDetection();
        }
        else
        {
            foreach(var item in detections)
            {
                item.ClaerWasHit();
                //���hit�б������Ƿ����Ҳȫ�����
                
            }
            isHited=false;//���������ж�Ҳ���
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    public void Impluse(int i = 0)
    {
        if (impulsePar.Length >3 && impulsePar[0]==1)
        {
            impulseSource.GenerateImpulse(WeaponDirection);
        }
    }
    /// <summary>
    /// ������������Ĺ�����Ч
    /// </summary>
    /// <param name="HitNum"></param>
    public void PlayHittedFx(int HitNum = 0)
    {
        ParticleSystem particle = HittedFx[0];
        particle.transform.position = this.GetComponentInChildren<WeaponFx>().transform.position;
        HittedFx[0].Play(true);
    }
    /// <summary>
    /// ��Ҫ��һ�����������ķ�����ֻ�ڵ�һ�λ���һ��characterINFOʱ�ŵ��ã���Ҫ������ǻ��е��ж����򣬻��е�collider
    /// </summary>
    public void Hitted()
    {
        Debug.Log("����");
        StartCoroutine(AdjustTimeScaleOverDuration(0.03f, 0.05f, 1f, 0.2f, this));
    }
    /// <summary>
    /// һ��ʱͣ���𶯵ĸ��Ϸ�����
    /// </summary>
    /// <param name="fadeInDuration"></param>
    /// <param name="fadeOutDuration"></param>
    /// <param name="duration"></param>
    /// <param name="targetTimeScale"></param>
    /// <param name="weaponManager"></param>
    /// <returns></returns>
    public System.Collections.IEnumerator AdjustTimeScaleOverDuration(float fadeInDuration, float fadeOutDuration, float duration, float targetTimeScale, WeaponManager weaponManager)
    {
        float initialTimeScale = Time.timeScale;
        float elapsedTime = 0f;

        // ����
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / fadeInDuration);
            Time.timeScale = Mathf.Lerp(initialTimeScale, targetTimeScale, normalizedTime);
            // ���������������Ҫ�����������߼�����

            // �ȴ�һ֡
            yield return null;
        }

        // ����Ŀ��ʱ������
        Time.timeScale = targetTimeScale;
        weaponManager.PlayHittedFx();
        // ����ʱ��
        yield return new WaitForSecondsRealtime(duration);

        // ����
        elapsedTime = 0f;

        //�����𶯺���Ч
        weaponManager.Impluse();
        //������Ҫ���������ط�������Ч��һ��������ĵ��������Ч������һ���ǹ�����ܻ�������
        //��Ҫ����ί��


        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / fadeOutDuration);
            Time.timeScale = Mathf.Lerp(targetTimeScale, 1f, normalizedTime);
            // ���������������Ҫ�����������߼�����

            // �ȴ�һ֡
            yield return null;
        }
        //һ����ʱͣ�����ʱ����ȥ�������������Ч����
        // �ָ�ԭʼ��ʱ������
        Time.timeScale = 1f;
    }
    /// <summary>
    /// �������������ϵ�������˶�����
    /// </summary>
    /// <returns></returns>
    public Vector3 GetWeaponDirectInverse(Transform HittedCharacterTransform)
    {
        return HittedCharacterTransform.InverseTransformDirection(WeaponDirection);
    }
}
