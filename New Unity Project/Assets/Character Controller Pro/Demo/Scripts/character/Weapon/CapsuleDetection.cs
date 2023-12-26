
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
    /// 这里返回的是这一帧击中的符合条件的物体
    /// </summary>
    /// <param name="isHited"></param>
    /// <returns></returns>
    public override List<Collider> GetDetection(out bool isHited)
    {
        List<Collider> result = new List<Collider>();
        //打出一个胶囊体，判断当前胶囊体中是否存在其他 collider，可选参数除了这三个以外，还可以使用 layermask 和QueryTriggerInteraction queryTriggerInteraction= QueryTriggerInteraction.UseGlobal （询问当前collider是否可以命中触发器）
        Collider[] hits = Physics.OverlapCapsule(startPoint.position, endPoint.position, radius);


        //求出来距离这个刀最近的地方
        foreach (var item in hits)
        {

            AgetHitBox hitBox;
            if (targetTags.Contains(item.tag) && item.TryGetComponent(out AttackReceive receive))
            {
                if (receive.isNormalReceive())
                {
                    hitBox = receive.CharacterInfo.hitBox;
                    if (hitBox && hitBox.agent && targetTags.Contains(hitBox.agent.tag) && !wasHit.Contains(hitBox.agent))//如果是可攻击对象，并且攻击对象中没有这个目标时
                    {
                        wasHit.Add(hitBox.agent);
                        result.Add(item);

                        //这样去添加物体
                        //这代表实际击中了，而且每击中一个人做一次的事件
                        if (!Weapon.HittedCharacter.Contains(hitBox.characterInfoOwner))
                        {
                            //添加攻击事件
                            HittedEvent?.Invoke(this, null);
                            //记录攻击到的人
                            Weapon.HittedCharacter.Add(hitBox.characterInfoOwner);
                            //调用一次//调用一个添加特效
                            //调用攻击到的人的受击方法
                            hitBox.characterInfoOwner.GetDamage(1, Weapon.transform.position, Weapon, item, IAgent.HitKind.ground);
                            //调用武器主人的攻击方法
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
