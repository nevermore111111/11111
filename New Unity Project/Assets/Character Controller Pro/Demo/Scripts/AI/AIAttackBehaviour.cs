using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using System;
using Unity.Mathematics;
using UnityEngine;

public class AIAttackBehaviour : CharacterAIBehaviour
{
    //写一个行为树，用来设计这个人物的攻击模式
    //1追踪
    //2到了之后，等待1-3秒 ：{1徘徊，或者等待}。之后判断
    //如果对方远离就靠近，否则开始攻击{期间对方攻击，就防御}
    //攻击之后继续回到等待
    //防御成功就会反击，反击分成两种，一种直接反击，一种等待反击。反击结束回到等待
    // virtual (optional)

    enum AIAttackState
    {
        awaitAttack,//这时随时防御攻击
        doAttack,
    }

    public float minAwaitTime = 0.5f;
    public float maxAwaitTime = 1.5f;

    [SerializeField]
    float reachDistance = 3f;
    AIAttackState currentAttackState;

    private float awaitTime;
    private Attack selectEnemyAttack;


    public override void EnterBehaviour(float dt)
    {
        if (CharacterActor.CharacterInfo.selectEnemy == null)
            CharacterActor.brain.SetAIBehaviour<AIFollowBehaviour>();
        //重置时间
        awaitTime = UnityEngine.Random.Range(minAwaitTime, maxAwaitTime);
        currentAttackState = AIAttackState.awaitAttack;
    }

    // abstract (mandatory)
    public override void UpdateBehaviour(float dt)
    {
        if (CharacterActor.CharacterInfo.selectEnemy != null)
        {
            JudgeDistance();
            //这时需要去执行正常逻辑了
            AwaitAttackUpdate(dt);
            doAttackUpdate(dt);
        }
    }
    /// <summary>
    /// 如果太远就切换当前的状态
    /// </summary>
    private void JudgeDistance()
    {
        if ((CharacterActor.CharacterInfo.selectEnemy.transform.position - transform.position).magnitude > reachDistance)
        {
            CharacterActor.brain.SetAIBehaviour<AIFollowBehaviour>();
        }
    }

    private void doAttackUpdate(float dt)
    {
        characterActions.attack.value = true;
    }

    private void AwaitAttackUpdate(float dt)
    {
        awaitTime -= dt;
        if (awaitTime < 0f)
        {
            return;
            //开始进入攻击状态
        }
        if(CharacterActor.CharacterInfo.selectEnemy.CharacterStateController.CurrentState is Attack)
        {
            selectEnemyAttack = (Attack)(CharacterActor.CharacterInfo.selectEnemy.CharacterStateController.CurrentState);
            if (selectEnemyAttack.isAttack == true)
            {
                SetDefendAction(true);
            }
        }
    }

    // virtual (optional)
    public override void ExitBehaviour(float dt)
    {

    }
}