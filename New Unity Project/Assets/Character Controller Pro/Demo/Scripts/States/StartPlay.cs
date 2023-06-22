using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlay : CharacterState
{

    protected override void Awake()
    {
        base.Awake();
    }
    override protected void Start()
    {
        base.Start();
    }
    public override void EnterBehaviour(float dt, CharacterState fromState)
    {
        CharacterActor.Animator.Play("GhostSamurai_APose_Equip02_Root");
    }
    public override void ExitBehaviour(float dt, CharacterState toState)
    {
        
    }
    override public void CheckExitTransition()
    {
        if(CharacterActor.Animator.GetBool("StartPlayEnd") == true)
        {
            CharacterStateController.EnqueueTransition<NormalMovement>();
            CharacterActor.Animator.CrossFade("StableGrounded", 0.5f);
        }
    }

    public override void UpdateBehaviour(float dt)
    {

    }
}
