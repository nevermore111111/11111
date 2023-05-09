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
        //打出一个胶囊体，判断当前胶囊体中是否存在其他 collider，可选参数除了这三个以外，还可以使用 layermask 和QueryTriggerInteraction queryTriggerInteraction= QueryTriggerInteraction.UseGlobal （询问当前collider是否可以命中触发器）
        Collider[] hits = Physics.OverlapCapsule(startPoint.position, endPoint.position, radius);
        foreach(var item in hits)
        {
            AgetHitBox hitBox;
            if(item.TryGetComponent(out hitBox)&&hitBox.agent&&targetTags.Contains(hitBox.agent.tag)&&!wasHit.Contains(hitBox.agent))//如果是可攻击对象，并且攻击对象中没有这个目标时
            {
                wasHit.Add(hitBox.agent);
                result.Add(item);
            }
        }
        return result;
    }
}
