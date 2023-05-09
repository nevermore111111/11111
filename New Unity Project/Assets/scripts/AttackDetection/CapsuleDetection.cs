using GluonGui.Dialog;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CapsuleDetection : Detection
{
    public Transform startPoint;
    public Transform endPoint;
    public float radius;

    public bool debug;

    public override List<Collider> GetDetection()
    {
        List<Collider> result = new List<Collider>();
        //���һ�������壬�жϵ�ǰ���������Ƿ�������� collider����ѡ�����������������⣬������ʹ�� layermask ��QueryTriggerInteraction queryTriggerInteraction= QueryTriggerInteraction.UseGlobal ��ѯ�ʵ�ǰcollider�Ƿ�������д�������
        Collider[] hits = Physics.OverlapCapsule(startPoint.position, endPoint.position, radius);
        foreach(var item in hits)
        {
            AgetHitBox hitBox;
            if(item.TryGetComponent(out hitBox)&&hitBox.agent&&targetTags.Contains(hitBox.agent.tag)&&!wasHit.Contains(hitBox.agent))//����ǿɹ������󣬲��ҹ���������û�����Ŀ��ʱ
            {
                wasHit.Add(hitBox.agent);
                result.Add(item);
            }
        }
        return result;
    }
}
