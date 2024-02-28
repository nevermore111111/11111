using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using System;
using Unity.Mathematics;
using UnityEngine;

public class AIAttackBehaviour : CharacterAIBehaviour
{
    //这是一个AI攻击模式，如果先检查自身和目标的距离，如果自身和目标的距离小于最大允许的距离，则继续，否则进入追踪的AI追踪模式。
    //在这个模式中，当进入这个状态的时候，先决定位置调整策略，并等待一段时间，在这段时间内，如果有人进行攻击，立即防御，并且重置等待时间，进入防御后，1s内的防御全部是完美防御，并且在下个判定时间并进行反击或者其他攻击，如果并没有完美防御，那么进行其他攻击
    //完成攻击后，看距离进入追踪或者攻击模式

    //ps：再写一个AI防御模式，在进入攻击之后，立刻进入防御模式。

    enum AIAttackState
    {
        awaitAttack,//这时随时防御攻击
        onAttack,
        doneAttack,
    }
    float refreshTime = 0.1f;
    float timer = 0;
    public bool ForceUpdate = false;
    public float minAwaitTime = 0.5f;
    public float maxAwaitTime = 1.5f;
    private bool isAdjustPos = false;

    [SerializeField]
    float reachDistance = 3f;
    AIAttackState currentAttackState;

    private float awaitTime;
    private Attack selectEnemyAttack;


    public override void EnterBehaviour(float dt)
    {
        CanAttackOrChangeState();
        //重置时间
        awaitTime = UnityEngine.Random.Range(minAwaitTime, maxAwaitTime) + refreshTime;
        currentAttackState = AIAttackState.awaitAttack;
        timer = refreshTime;
        characterActions.Reset();
        isAdjustPos = false;
    }

    // abstract (mandatory)
    public override void UpdateBehaviour(float dt)
    {
        if (timer >= refreshTime)
        {
            timer = 0;
            //执行一次逻辑
            if (CanAttackOrChangeState())
            {
                if (!AwaitAttackDeltaTime(timer)/*这个是当前是否防御*/)
                {
                    if (currentAttackState != AIAttackState.doneAttack)//当前的攻击没有进行完成
                    {
                        doAttackDeltaTime(timer);
                    }
                }
            }
            //这时需要去执行正常逻辑了
        }
        timer += dt;
        if (ForceUpdate)
        {
            timer += refreshTime;
        }
    }

    private bool CanAttackOrChangeState()
    {
        if (!CanAttack())
        {
            CharacterActor.brain.SetAIBehaviour<AIFollowBehaviour>();
            return false;
        }
        return true;
        bool CanAttack()
        {
            //没人，或人太远
            if (CharacterActor.CharacterInfo.selectEnemy == null || (CharacterActor.CharacterInfo.selectEnemy.transform.position - CharacterActor.transform.position).magnitude > reachDistance)
            {
                return false;
            }
            return true;
        }
    }

    private void doAttackDeltaTime(float timer)
    {
        characterActions.Reset();
        characterActions.attack.value = true;
    }

    private bool AwaitAttackDeltaTime(float timer)
    {
        awaitTime -= timer;
        if (awaitTime < 0f)
        {
            return false;
            //开始进入攻击状态
        }
        else if (!isAdjustPos && CharacterActor.CharacterInfo.selectEnemy != null)
        {
            //进行调整
            isAdjustPos = true;
            characterActions.Reset();
            //这个重置之后，要进入冲刺
            characterActions.evade.value = true;
            SetMovementAction(Quaternion.AngleAxis(UnityEngine.Random.Range(-45f, 45f), Vector3.up) *(-CharacterActor.Forward));
        }
        else if (GetIsEnemyAtttack())
        {
            characterActions.Reset();
            SetDefendAction(true);
        }
        return true;//这个是return防御时间成功。
        bool GetIsEnemyAtttack()
        {
            return CharacterActor.CharacterInfo.selectEnemy.characterActor.CharacterInfo.attackAndDefendInfo.isAtttack;
        }
        //if (CharacterActor.CharacterInfo.selectEnemy.CharacterStateController.CurrentState is Attack)
        //{
        //    selectEnemyAttack = (Attack)(CharacterActor.CharacterInfo.selectEnemy.CharacterStateController.CurrentState);
        //    if (selectEnemyAttack.isAttack == true)
        //    {
        //        SetDefendAction(true);
        //    }
        //}
    }



    // virtual (optional)
    public override void ExitBehaviour(float dt)
    {

    }
}