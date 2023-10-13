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
        if(targetGroup.m_Targets.Length > 0)
        {
             currentWeight = targetGroup.m_Targets[0].weight;
        }
        
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
        if(targetGroup.m_Targets.Length >targetIndex -1)
        targetGroup.m_Targets[targetIndex].weight = newWeight;
    }




    private void OnTriggerEnter(Collider other)
    {

        AttackReceive attackReceive;
        if (other.gameObject.CompareTag("enemy") && other.TryGetComponent<AttackReceive>(out attackReceive) == true && !mainCharacter.enemies.Contains(attackReceive.CharacterInfo))
        {
            CharacterInfo characterInfo = attackReceive.CharacterInfo;
            if (characterInfo != null)
            {
                mainCharacter.enemies.Add(characterInfo);
                int indexNum = targetGroup.FindMember(characterInfo.transform);
                if (indexNum == -1)
                {
                    targetGroup.AddMember(characterInfo.transform, 0, characterInfo.cameraRadius);
                }
                indexNum = targetGroup.FindMember(characterInfo.transform);
                StartCoroutine(AdjustTargetWeight(1f, 1f, indexNum));
            }
        }
    }



    private void OnTriggerExit(Collider other)
    {
        AttackReceive attackReceive;
        if (other.gameObject.CompareTag("enemy") && other.TryGetComponent<AttackReceive>(out attackReceive) == true && mainCharacter.enemies.Contains(attackReceive.CharacterInfo))
        {
            mainCharacter.enemies.Remove(attackReceive.CharacterInfo);
            int targetIndex = targetGroup.FindMember(attackReceive.CharacterInfo.transform);
            //targetGroup.RemoveMember(enemyTransform);
            // 在敌人离开范围时，延迟删除目标并逐渐减小权重
            //StartCoroutine(DelayedRemoveMember(enemyTransform, 1f));
            StartCoroutine(AdjustTargetWeight(0f, 1f, targetIndex));
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



 

}
