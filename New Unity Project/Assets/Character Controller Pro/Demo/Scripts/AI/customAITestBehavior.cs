using Lightbug.CharacterControllerPro.Implementation;
using UnityEngine;

public class customAITestBehavior : CharacterAIBehaviour
{
    private GameObject character;
    //我要做的，直接添加主角，有主角，判定范围。
    //无反应范围，

    public customAITestBehavior(GameObject character)
    {
        this.character = character;
    }
    public override void UpdateBehaviour(float dt)
    {
        // 感知环境
        PerceptionInfo perceptionInfo = PerceiveEnvironment();

        // 收集信息
        CollectedData collectedData = CollectData(perceptionInfo);

        // 制定目标
        Target target = SetTarget(collectedData);

        // 决策制定
        ActionPlan actionPlan = MakeDecision(target);

        // 执行行动
        ExecuteAction(actionPlan);
    }

    public override void EnterBehaviour(float dt)
    {
        base.EnterBehaviour(dt);
    }

    public override void ExitBehaviour(float dt)
    {
        base.ExitBehaviour(dt);
    }

    private PerceptionInfo PerceiveEnvironment()
    {
        // 感知环境的具体实现
        return new PerceptionInfo();
    }

    private CollectedData CollectData(PerceptionInfo perceptionInfo)
    {
        // 收集信息的具体实现
        return new CollectedData();
    }

    private Target SetTarget(CollectedData collectedData)
    {
        // 制定目标的具体实现
        return new Target();
    }

    private ActionPlan MakeDecision(Target target)
    {
        // 决策制定的具体实现
        return new ActionPlan();
    }

    private void ExecuteAction(ActionPlan actionPlan)
    {
        // 执行行动的具体实现
    }
}

public class PerceptionInfo
{
    // 感知到的环境信息的定义
}

public class CollectedData
{
    // 收集到的信息的定义
}

public class Target
{
    // 目标的定义
}

public class ActionPlan
{
    // 行动计划的定义
}
