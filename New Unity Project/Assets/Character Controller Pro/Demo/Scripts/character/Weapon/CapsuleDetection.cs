
using DG.Tweening;
using System;
using System.Collections.Generic;

using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CapsuleDetection : Detection
{
    public Transform startPoint;
    public Transform endPoint;
    public float radius;
    public bool debug;

    public event EventHandler HittedEvent;

    private void OnDrawGizmos()
    {
        if (debug && startPoint && endPoint)
        {
            Vector3 direction = endPoint.position - startPoint.position;
            float length = direction.magnitude;
            direction.Normalize();

            if (length >= 0)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(startPoint.position, radius);
                Gizmos.DrawWireSphere(endPoint.position, radius);

                Vector3 perpendicular = Vector3.Cross(direction, Vector3.up).normalized;
                Gizmos.DrawLine(startPoint.position + perpendicular * radius, endPoint.position + perpendicular * radius);
                Gizmos.DrawLine(startPoint.position - perpendicular * radius, endPoint.position - perpendicular * radius);

                perpendicular = Vector3.Cross(perpendicular, direction).normalized;
                Gizmos.DrawLine(startPoint.position + perpendicular * radius, endPoint.position + perpendicular * radius);
                Gizmos.DrawLine(startPoint.position - perpendicular * radius, endPoint.position - perpendicular * radius);
            }
        }
    }

    /// <summary>
    /// ���ﷵ�ص�����һ֡���еķ�������������
    /// </summary>
    /// <param name="isHited"></param>
    /// <returns></returns>
    public override List<Collider> GetDetection(out bool isHited)
    {
        List<Collider> result = new List<Collider>();
        //���һ�������壬�жϵ�ǰ���������Ƿ�������� collider����ѡ�����������������⣬������ʹ�� layermask ��QueryTriggerInteraction queryTriggerInteraction= QueryTriggerInteraction.UseGlobal ��ѯ�ʵ�ǰcollider�Ƿ�������д�������
        Collider[] hits = Physics.OverlapCapsule(startPoint.position, endPoint.position, radius);


        //������������������ĵط�
        foreach (var item in hits)
        {

            AgetHitBox hitBox;
            if (targetTags.Contains(item.tag) && item.TryGetComponent(out AttackReceive receive))
            {
                if (receive.isNormalReceive())
                {
                    hitBox = receive.CharacterInfo.hitBox;
                    if (hitBox && hitBox.agent && targetTags.Contains(hitBox.agent.tag) && !wasHit.Contains(hitBox.agent))//����ǿɹ������󣬲��ҹ���������û�����Ŀ��ʱ
                    {
                        wasHit.Add(hitBox.agent);
                        result.Add(item);

                        //����ȥ�������
                        //�����ʵ�ʻ����ˣ�����ÿ����һ������һ�ε��¼�
                        if (!Weapon.HittedCharacter.Contains(hitBox.characterInfoOwner))
                        {
                            //��ӹ����¼�
                            HittedEvent?.Invoke(this, null);
                            //��¼����������
                            Weapon.HittedCharacter.Add(hitBox.characterInfoOwner);
                            //����һ��//����һ�������Ч
                            //���ù��������˵��ܻ�����
                            hitBox.characterInfoOwner.GetDamage(1, Weapon.transform.position, Weapon, item, IAgent.HitKind.ground);
                            //�����������˵Ĺ�������
                            Weapon.weaponOwner.HitOther(Weapon);
                        }
                    }
                }
                else if (receive.isCriticalReceive())
                {

                }
                else if (receive.isExtremeEvade())
                {

                }

            }

        }
        if (result.Count != 0)
        {
            isHited = true;
        }
        else
        {
            isHited = false;
        }
        return result;
    }
}
