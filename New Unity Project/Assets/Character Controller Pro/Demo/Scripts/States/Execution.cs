using Lightbug.CharacterControllerPro.Implementation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Execution : CharacterState
{
    public override void EnterBehaviour(float dt, CharacterState fromState)
    {
        //去播放动画
        //根据处决的方位，去播放不同的动画
    }
    public override void ExitBehaviour(float dt, CharacterState toState)
    {

    }
    public override void UpdateBehaviour(float dt)
    {
        //setIK
    }
    public override void CheckExitTransition()
    {
        //暂时只有地面的处决
        if(!CharacterActor.IsGrounded||CharacterActor.CharacterInfo.selectEnemy == null)
        {
            return;
        }
    }
}
