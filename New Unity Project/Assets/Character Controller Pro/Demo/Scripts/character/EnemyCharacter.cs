using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : CharacterInfo, IAgent
{
     CharacterActor characterActor;
    public float targetGroupSize = 1;
    public float targetHp = 10000;

    protected override  void Awake()
    {
        base.Awake();
        characterActor = GetComponentInParent<CharacterActor>();
    }

    public void GetDamage(float damage, Vector3 pos)
    {
        //这个地方需要 1 细化规则 播放那个动画
        //需要在已经进入当前状态的时候是否重新播放动画

        //characterActor.Animator.Play("GetDamage", 0, 0); // 重置并播放动画
        Debug.Log("jizhongle");
    }


}
