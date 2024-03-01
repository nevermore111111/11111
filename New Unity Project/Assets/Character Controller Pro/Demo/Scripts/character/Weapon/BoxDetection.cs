using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
[RequireComponent(typeof(KnifeManager))]
public class BoxDetection : Detection
{
    public Vector3 boxCenter;
    public Vector3 halfExtents;
    public Quaternion orientation;
    public bool debug;
    public event EventHandler HittedEvent;

    KnifeManager knifeManager;

    public override void Awake()
    {
        base.Awake();
        knifeManager = GetComponent<KnifeManager>();
    }


    public override void ResetBeForeHit()
    {
        knifeManager.ResetBeForeHit();
    }


    public override List<Collider> GetDetection(out bool isHited)
    {
        List<Collider> result = new List<Collider>();
        //更正当前帧的位置
        knifeManager.BeforeBoxUpdate();

        Collider[] hits = Physics.OverlapBox(boxCenter, halfExtents, orientation);

        foreach (var item in hits)
        {
            AgetHitBox hitBox;
            if (targetTags.Contains(item.tag) && item.TryGetComponent(out AttackReceive receive))
            {
                if (receive.isNormalReceive())
                {
                    hitBox = receive.CharacterInfo.hitBox;
                    if (hitBox && hitBox.agent && targetTags.Contains(hitBox.agent.tag) && !wasHit.Contains(hitBox.agent))
                    {
                        wasHit.Add(hitBox.agent);
                        result.Add(item);

                        if (!Weapon.HittedCharacter.Contains(hitBox.characterInfoOwner))
                        {
                            HittedEvent?.Invoke(this, null);
                            Weapon.HittedCharacter.Add(hitBox.characterInfoOwner);

                            hitBox.characterInfoOwner.GetDamage(1, Weapon.transform.position, Weapon, item, IAgent.HitKind.ground);
                            Weapon.weaponOwner.HitOther(Weapon);
                        }
                    }
                }
                else if (receive.isCriticalReceive())
                {
                    receive.HitStart();
                }
                else if (receive.isExtremeEvade())
                {
                    // Handle extreme evade case
                }
            }
        }

        isHited = result.Count != 0;
        return result;
    }

    private void OnDrawGizmos()
    {
        if (!debug)
            return;
#if UNITY_EDITOR
        if (!Application.isPlaying) return;

#endif
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, orientation, Vector3.one);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, 2 * halfExtents);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
