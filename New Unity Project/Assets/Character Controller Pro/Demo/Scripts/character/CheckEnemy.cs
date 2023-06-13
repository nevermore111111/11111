using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Timeline;
using System.Linq;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;

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
    public CharacterActor characterActor ;
    private NormalMovement NormalMovement;
    private float MoveDeceleration;
    private void Awake()
    {
        NormalMovement = characterActor.GetComponentInChildren<NormalMovement>();
        float currentWeight = targetGroup.m_Targets[0].weight;
        MoveDeceleration = NormalMovement.planarMovementParameters.stableGroundedDeceleration;
    }
    IEnumerator AdjustTargetWeight(float newWeight, float duration, int targetIndex)
    {
        float elapsedTime = 0;
        float startWeight = targetGroup.m_Targets[targetIndex].weight;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            if(targetGroup.m_Targets.Length >= targetIndex)
            targetGroup.m_Targets[targetIndex].weight = Mathf.Lerp(startWeight, newWeight, t);
            yield return null;
        }

        targetGroup.m_Targets[targetIndex].weight = newWeight;
    }





    private void OnTriggerEnter(Collider other)
    {
        AddEnemy(other);
        if(mainCharacter.enemys.Count != 0)
        {
            NormalMovement.planarMovementParameters.stableGroundedDeceleration = MoveDeceleration * 1.5f;
        }
        if (other.gameObject.CompareTag("enemy") && !mainCharacter.enemys.Contains(other.gameObject.GetComponentInParent<CharacterInfo>()))
        {
            // 在敌人重新进入范围时，停止延迟删除协程
            Transform enemyTransform = other.transform;
            int targetIndex = FindTargetIndex(enemyTransform);

            if (targetIndex != -1)
            {
                StopCoroutine(DelayedRemoveMember(enemyTransform, 1f));

                // 在敌人重新进入范围时，逐渐增加权重
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

            // 在敌人离开范围时，延迟删除目标并逐渐减小权重
            StartCoroutine(DelayedRemoveMember(enemyTransform, 1f));
            StartCoroutine(AdjustTargetWeight(0f, 1f, targetIndex));
        }
        if(mainCharacter.enemys.Count == 0)
        {
            NormalMovement.planarMovementParameters.stableGroundedDeceleration = MoveDeceleration ;
        }
    }

    IEnumerator DelayedRemoveMember(Transform enemyTransform, float delay)
    {
        yield return new WaitForSeconds(delay);

        // 检查敌人是否在范围内，如果不在范围内，则从目标组中移除
        if (!enemyTransform.GetComponent<SphereCollider>().bounds.Contains(targetGroup.transform.position))
        {
            targetGroup.RemoveMember(enemyTransform);
        }
    }
    private void Update()
    {
        //
        MovePosition();

        

    }


    /// <summary>
    /// 修改物体的位置和人物重合
    /// </summary>
    private void MovePosition()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, Character.transform.position, MoveToCharacterSpeed * Time.deltaTime);
    }




    //摄像机增加
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
                Debug.Log("这里null");
            }
        }
    }



 

}
