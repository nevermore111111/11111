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

    private void Awake()
    {

        float currentWeight = targetGroup.m_Targets[0].weight;
    }
    IEnumerator AdjustTargetWeight(float newWeight, float duration, int targetIndex)
    {
        float elapsedTime = 0;
        float startWeight = currentWeight;

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
    }

    private void AddEnemy(Collider other)
    {
        if (other.gameObject.CompareTag("enemy") & (!mainCharacter.enemys.Contains(other.gameObject.GetComponentInParent<CharacterInfo>())))
        {
            CharacterInfo characterInfo = other.gameObject.GetComponentInParent<CharacterInfo>();
            if (characterInfo != null)
            {
                mainCharacter.enemys.Add(other.gameObject.GetComponentInParent<CharacterInfo>());
                if (targetGroup.FindMember(other.transform) == -1)
                {
                    targetGroup.AddMember(other.transform, 1, other.GetComponentInParent<CharacterInfo>().GetComponent<SphereCollider>().radius);
                }
            }
            else
            {
                Debug.Log("这里null");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("enemy") && mainCharacter.enemys.Contains(other.gameObject.GetComponentInParent<CharacterInfo>()))
        {
            mainCharacter.enemys.Remove(other.gameObject.GetComponentInParent<CharacterInfo>());
            StartCoroutine(DelayedEnemyRemoval(other.transform));
        }
    }
    private void Update()
    {
        //
        SetCamera();
        MovePosition();
    }
    /// <summary>
    /// 设置主摄像机的优先级，当选择单位中没有敌人的时候，那么将主摄像机的优先级提升。
    /// </summary>
    private void SetCamera()
    {
        //
        if (mainCharacter.enemys.Count == 0)
        {
           // MainCamera.Priority = 100;
        }
        else
        {
            //MainCamera.Priority = 5;
        }

    }

    /// <summary>
    /// 修改物体的位置和人物重合
    /// </summary>
    private void MovePosition()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, Character.transform.position, 10.0f * Time.deltaTime);
    }

    //摄像机增加
    private void AddEnemy(Transform obj, float weight, Collider other)
    {
        CharacterInfo character = other.GetComponentInParent<CharacterInfo>();

        //targetGroup.AddMember(newTarget.target, newTarget.weight, newTarget.radius);
        targetGroup.AddMember(obj, 1, obj.GetComponentInChildren<CharacterInfo>().characterSphere.radius);
    }



    IEnumerator DelayedEnemyRemoval(Transform enemyTransform)
    {
        yield return new WaitForSeconds(1f);
        if (!Physics.CheckSphere(enemyTransform.position, 5))
        {
            targetGroup.RemoveMember(enemyTransform);
        }
    }

}
