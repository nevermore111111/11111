using Lightbug.CharacterControllerPro.Implementation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Execution : CharacterState
{
    public override void EnterBehaviour(float dt, CharacterState fromState)
    {
        //ȥ���Ŷ���
        //���ݴ����ķ�λ��ȥ���Ų�ͬ�Ķ���
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
        //��ʱֻ�е���Ĵ���
        if(!CharacterActor.IsGrounded||CharacterActor.CharacterInfo.selectEnemy == null)
        {
            return;
        }
    }
}
