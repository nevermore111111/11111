using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : CharacterInfo, IAgent
{
     CharacterActor characterActor;
    public float targetGroupSize = 1;
    protected override  void Awake()
    {
        base.Awake();
        characterActor = GetComponentInParent<CharacterActor>();
    }

    public void GetDamage(float damage, Vector3 pos)
    {
        //����ط���Ҫ 1 ϸ������ �����Ǹ�����
        //��Ҫ���Ѿ����뵱ǰ״̬��ʱ���Ƿ����²��Ŷ���

        //characterActor.Animator.Play("GetDamage", 0, 0); // ���ò����Ŷ���
        Debug.Log("jizhongle");
    }


}
