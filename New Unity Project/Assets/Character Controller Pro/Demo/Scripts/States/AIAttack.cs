using JetBrains.Annotations;
using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
using Rusk;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// 
/// <summary>
public class AIAttack : CharacterState
{
    [OnCollectionChanged("AttacksRename")]
    public List<AIAttackData> attacks;
    public float CharacterAttackDistance;
    public bool isAttack;
    public bool isReadyToAttack;
    public bool canChangeState =false;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }
    public override void EnterBehaviour(float dt, CharacterState fromState)
    {
        Debug.Log("开始");
        string targetAttack = ChooseAttack(attacks);
        AnimFun(targetAttack);
        canChangeState = false;
    }

    private void AnimFun(string targetAnim)
    {
        if (CharacterActor != null)
        {
            Debug.LogError("这里没写,下一行要播放对应动画层的攻击动画");
            string targetTotalAnimName =  targetAnim;
            CharacterActor.Animator.CrossFade(targetTotalAnimName,0.25f);
        }
    }

    public override void ExitBehaviour(float dt, CharacterState toState)
    {
        
    }

    public override void UpdateBehaviour(float dt)
    {
        GetSelectAttack();
    }

    private void GetSelectAttack()
    {
        if (isReadyToAttack)
        {
            CharacterStateController.EnqueueTransition<AIAttack>();
        }
    }

    public override void CheckExitTransition()
    {
        if (canChangeState)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
        }
    }



    //_________________________________分割线_____________________________________________
    //_________________________________分割线_____________________________________________
    //_________________________________分割线_____________________________________________

    /// <summary>
    /// 给attacks自动命名的方法
    /// </summary>
    public void AttacksRename()
    {
        foreach (var attack in attacks) 
        {
            if(attack != null&&attack.attackName.IsNullOrEmpty())
            {
                attack.attackName = "BaseLayer.AIAttack.Attack" + (attacks.IndexOf(attack)+1).ToString();
            }
        }
    }

    public string ChooseAttack(List<AIAttackData> attacks)
    {
        // 计算总权重
        int totalWeight = 0;
        foreach (var attack in attacks)
        {
            totalWeight += attack.attackChooseWeight;
        }

        // 生成随机数
        int randomValue = UnityEngine.Random.Range(0, totalWeight);

        // 根据随机数选择攻击
        int cumulativeWeight = 0;
        foreach (var attack in attacks)
        {
            cumulativeWeight += attack.attackChooseWeight;
            if (randomValue < cumulativeWeight)
            {
                return attack.attackName;
            }
        }
        // 如果出现异常情况，返回第一个攻击
        return attacks[0].attackName;
    }

}

[Serializable]
public class AIAttackData
{
    public string attackName;
    public int attackChooseWeight;
}
