using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeAnimBehavior : StateMachineBehaviour
{
    public int Kind;
    private NormalMovement normalMovement;
    //private CharacterActor actor;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (normalMovement == null)
        {
            normalMovement = animator.transform.parent.parent.GetComponentInChildren<NormalMovement>();
        }
        //if(actor == null)
        //{
        //    actor = animator.GetComponentInParent<CharacterActor>();
        //}
        //switch (Kind)
        //{
        //    case 1:
        //        //animator.SetLayerWeight(2, 1f);
        //        break;
        //    case 2:
        //        animator.SetLayerWeight(2, 0f);
        //        break;
        //    case 3:
        //        break;
        //}
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (Kind)
        {
            case 1:
                animator.SetBool(CharacterState.stopParameter, false);
                break;
            case 2:
             
                break;
            case 3:
                break;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    //switch (Kind)
    //    //{
    //    //    ////case 1:
    //    //    ////   break;
    //    //    ////case 2:
    //    //    ////   
    //    //    ////    break;
    //    //    //case 3:
    //    //    //   
    //    //    //    break;
    //    //}

    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
