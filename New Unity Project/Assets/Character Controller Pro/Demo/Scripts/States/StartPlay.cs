using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVRM10;

public class StartPlay : CharacterState
{
    public bool changeState = false;
    private bool actorOnlyOnce = true;

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
        CharacterActor.Animator.Play("Idle");
    }
    public override void ExitBehaviour(float dt, CharacterState toState)
    {

    }
    override public void CheckExitTransition()
    {
        if (changeState)
        {
            CharacterStateController.EnqueueTransition<AttackOnGround>();
            //CharacterStateController.EnqueueTransition<NormalMovement>();
        }

    }

    public override void UpdateBehaviour(float dt)
    {
        if (CharacterActions.attack.value&&actorOnlyOnce)
        {
            CharacterActor.Animator.Play("GhostSamurai_APose_Equip02_Root");
            actorOnlyOnce = false;  
        }
        BaseProcessVelocity(dt);
        Debug.Log(CharacterActor.Velocity);
   }
    public void ChangeState()
    {
        changeState = true;
    }
}
