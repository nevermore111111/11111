using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Timeline;
using System.Linq;

/// <summary>
/// 
/// <summary>
public class CheckEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject Character;
    [SerializeField]
    private MainCharacter mainCharacter;
    [SerializeField]
    private CinemachineTargetGroup targetGroup;
    float currentWeight;
    public CinemachineFreeLook MainCamera;
    private List<Transform> enemiesInRange = new List<Transform>();
    public float MoveToCharacterSpeed = 10.0f;

    private void Awake()
    {

        float currentWeight = targetGroup.m_Targets[0].weight;
    }
    IEnumerator AdjustTargetWeight(float newWeight, float duration, int targetIndex)
    {
        float elapsedTime = 0;
        float startWeight = targetGroup.m_Targets[targetIndex].weight;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            targetGroup.m_Targets[targetIndex].weight = Mathf.Lerp(startWeight, newWeight, t);
            yield return null;
        }

        targetGroup.m_Targets[targetIndex].weight = newWeight;
    }





    private void OnTriggerEnter(Collider other)
    {
        AddEnemy(other);
        if (other.gameObject.CompareTag("enemy") && !mainCharacter.enemys.Contains(other.gameObject.GetComponentInParent<CharacterInfo>()))
        {
            // �ڵ������½��뷶Χʱ��ֹͣ�ӳ�ɾ��Э��
            Transform enemyTransform = other.transform;
            int targetIndex = FindTargetIndex(enemyTransform);

            if (targetIndex != -1)
            {
                StopCoroutine(DelayedRemoveMember(enemyTransform, 1f));

                // �ڵ������½��뷶Χʱ��������Ȩ��
                StartCoroutine(AdjustTargetWeight(1f, 1f, targetIndex));
            }
        }
    }
    private int FindTargetIndex(Transform target)
    {
        for (int i = 0; i < targetGroup.m_Targets.Length; i++)
        {
            if (targetGroup.m_Targets[i].target == target)
            {
                return i;
            }
        }
        return -1;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("enemy") && mainCharacter.enemys.Contains(other.gameObject.GetComponentInParent<CharacterInfo>()))
        {
            mainCharacter.enemys.Remove(other.gameObject.GetComponentInParent<CharacterInfo>());
            Transform enemyTransform = other.gameObject.GetComponentInParent<Transform>();
            int targetIndex = targetGroup.FindMember(enemyTransform);

            // �ڵ����뿪��Χʱ���ӳ�ɾ��Ŀ�겢�𽥼�СȨ��
            StartCoroutine(DelayedRemoveMember(enemyTransform, 1f));
            StartCoroutine(AdjustTargetWeight(0f, 1f, targetIndex));
        }
    }

    IEnumerator DelayedRemoveMember(Transform enemyTransform, float delay)
    {
        yield return new WaitForSeconds(delay);

        // �������Ƿ��ڷ�Χ�ڣ�������ڷ�Χ�ڣ����Ŀ�������Ƴ�
        if (!enemyTransform.GetComponent<SphereCollider>().bounds.Contains(targetGroup.transform.position))
        {
            targetGroup.RemoveMember(enemyTransform);
        }
    }
    private void Update()
    {
        //
        SetCamera();
        MovePosition();

        

    }
    /// <summary>
    /// ����������������ȼ�����ѡ��λ��û�е��˵�ʱ����ô��������������ȼ�������
    /// </summary>
    private void SetCamera()
    {
        //
        if (mainCharacter.enemys.Count == 0)
        {
            MainCamera.Priority = 100;
        }
        else
        {
            MainCamera.Priority = 5;
        }

    }

    /// <summary>
    /// �޸������λ�ú������غ�
    /// </summary>
    private void MovePosition()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, Character.transform.position, MoveToCharacterSpeed * Time.deltaTime);
    }




    //���������
    private void AddEnemy(Collider other)
    {
        if (other.gameObject.CompareTag("enemy") && !mainCharacter.enemys.Contains(other.gameObject.GetComponentInParent<CharacterInfo>()))
        {
            CharacterInfo characterInfo = other.gameObject.GetComponentInParent<CharacterInfo>();
            if (characterInfo != null)
            {
                mainCharacter.enemys.Add(characterInfo);
                if (targetGroup.FindMember(other.transform) == -1)
                {
                    targetGroup.AddMember(other.transform, 0, other.GetComponentInParent<CharacterInfo>().GetComponent<SphereCollider>().radius);
                    StartCoroutine(AdjustTargetWeight(1f, 1f, targetGroup.m_Targets.Length - 1));
                }
            }
            else
            {
                Debug.Log("����null");
            }
        }
    }



 

}
