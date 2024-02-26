using Cinemachine;
using DG.Tweening;
using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyAll : MonoBehaviour
{
    public CharacterActor CharacterActor;
    private CinemachineTargetGroup Group;
    public string EnemyTag = "custom";
    public float TargetGroupWeight = 1.0f;
    public float ChangeTargetWeightDuration = 0.5f;
    public CinemachineVirtualCamera VirtualCamera;
    public CinemachineBrain brain;
    private void Awake()
    {
        CharacterActor = GetComponentInParent<CharacterActor>();
        VirtualCamera = GameObject.Find("subCamera").GetComponent<CinemachineVirtualCamera>();
        brain = FindFirstObjectByType<CinemachineBrain>();
    }
    private void Start()
    {
        ResetEnemyTag();
    }

    /// <summary>
    /// 进来的物体加到选择目标中
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        AttackReceive attackReceive;
        if (other.gameObject.CompareTag(EnemyTag) && other.TryGetComponent(out attackReceive) == true && attackReceive.isNormalReceive() && !CharacterActor.CharacterInfo.enemies.Contains(attackReceive.CharacterInfo))
        {
            CheckTarget(attackReceive, TargetGroupWeight);
        }
        if (brain?.ActiveVirtualCamera != (ICinemachineCamera)VirtualCamera && !brain.IsBlending)//在主摄像机不是他并且非混合状态下
        {
            VirtualCamera.ForceCameraPosition(brain.transform.position, brain.transform.rotation);
        }
    }

    /// <summary>
    /// 出去的目标被移除
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        AttackReceive attackReceive;
        if (other.gameObject.CompareTag(EnemyTag) && other.TryGetComponent(out attackReceive) == true && attackReceive.isNormalReceive() && CharacterActor.CharacterInfo.enemies.Contains(attackReceive.CharacterInfo))
        {
            CheckTarget(attackReceive, 0f);
        }
    }


    //_________________________________________分隔线_____________________________________________________________________________
    //_________________________________________分隔线_____________________________________________________________________________
    //_________________________________________分隔线_____________________________________________________________________________



    private void ResetEnemyTag()
    {
        if (EnemyTag == "custom")
        {
            Debug.Log("因为EnemyTag 还是custom，所以自动修改");
            if (CharacterActor.IsPlayer)
                EnemyTag = "enemy";
            else
                EnemyTag = "Player";
        }
    }
    /// <summary>
    /// 把对应的人物，从target中添加或者删除
    /// </summary>
    /// <param name="attackReceive"></param>
    /// <param name="targetWeight"></param>
    private void CheckTarget(AttackReceive attackReceive, float targetWeight)
    {
        //目标是0，这代表要把这个对象移动出去
        if (targetWeight <= 0f)
        {
            if (!CharacterActor.IsPlayer)//非主角
            {
                //非主角移除这个人
                CharacterActor.CharacterInfo.enemies.Remove(attackReceive.CharacterInfo);
            }
            //如果是主角需要去逐渐修改权重，到达0的时候去移除
            else if (CharacterActor.IsPlayer && Group != null)
            {
                //我发现如果这个地方只在第一帧确认了targetTransform，但是如果Group的成员发生了变化，对应索引的目标也就变了，这时候应该每帧都去确认targetTransform
                //为了避免每帧都去确认对应的targetTransform，所以我现在不做删除，只是在每次进行权重操作完成的时候，检测如果没有权重正在执行，这时候再去移除目标
                int targetTransform = Group.FindMember(attackReceive.CharacterInfo.transform);
                if (targetTransform != -1)
                {
                    DOTween.To(() => Group.m_Targets[targetTransform].weight, value =>
                    {
                        Group.m_Targets[targetTransform].weight = value;
                    }, targetWeight, ChangeTargetWeightDuration)
                    .SetId("CinemachineTargetGroup").OnComplete(() =>
                    {
                        RemoveTargetWeightEqualZero();
                    });
                }
            }
        }
        else if (targetWeight > 0f)
        {
            //把目标增加到自身敌人列表中
            if (!CharacterActor.CharacterInfo.enemies.Contains(attackReceive.CharacterInfo))
                CharacterActor.CharacterInfo.enemies.Add(attackReceive.CharacterInfo);
            //如果是主角需要去修改摄像机
            if (CharacterActor.IsPlayer && Group != null)
            {
                //我发现如果这个地方只在第一帧确认了targetTransform，但是如果Group的成员发生了变化，对应索引的目标也就变了，这时候应该每帧都去确认targetTransform
                //为了避免每帧都去确认对应的targetTransform，所以我现在不做删除，只是在每次进行权重操作完成的时候，检测如果没有权重正在执行，这时候再去移除目标
                int targetTransform = Group.FindMember(attackReceive.CharacterInfo.transform);
                if (targetTransform != -1)
                {
                    DOTween.To(() => Group.m_Targets[targetTransform].weight, value =>
                    {
                        Group.m_Targets[targetTransform].weight = value;
                    }, targetWeight, ChangeTargetWeightDuration)
                    .SetId("CinemachineTargetGroup").OnComplete(() =>
                    {
                        RemoveTargetWeightEqualZero();
                    });
                }
            }
        }

    }

    /// <summary>
    /// 移除权重等于0的物体
    /// </summary>
    private void RemoveTargetWeightEqualZero()
    {
        //检测是否正在调整，如果正在调整，那我就不再去删除，如果没有在调整的，我去检查空的成员或者权重为零的成员，把他们移除
        if (!DOTween.IsTweening("CinemachineTargetGroup"))
        {
            for (int i = 0; i < Group.m_Targets.Length; i++)
            {
                if (Group.m_Targets[i].weight == 0)
                {
                    Group.RemoveMember(Group.m_Targets[i].target);
                }
            }
        }
    }
}
