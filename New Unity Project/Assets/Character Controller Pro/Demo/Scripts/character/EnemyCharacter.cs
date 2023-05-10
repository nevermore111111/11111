using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : CharacterInfo, IAgent
{
     CharacterActor characterActor;
    protected override  void Awake()
    {
        base.Awake();
        characterActor = GetComponentInParent<CharacterActor>();
    }

    public void GetDamage(float damage, Vector3 pos)
    {
        //����ط���Ҫ 1 ϸ������ �����Ǹ�����
        //��Ҫ���Ѿ����뵱ǰ״̬��ʱ���Ƿ����²��Ŷ���
        characterActor.Animator.CrossFade("Lucy_Hit_F_Inplace", 0.2f);
    }


}
