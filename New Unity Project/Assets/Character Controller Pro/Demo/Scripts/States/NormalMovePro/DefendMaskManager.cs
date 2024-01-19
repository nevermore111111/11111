using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DefendMaskManager : StateMachineBehaviour
{
    public int Kind;
    private  NormalMovement normalMovement;
    //private CharacterActor actor;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(normalMovement == null)
        {
            normalMovement = animator.transform.parent.parent.GetComponentInChildren<NormalMovement>();
        }
        animator.SetBool(CharacterState.stopParameter, false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    switch (Kind)
    //    {
    //        case 1:
    //            //animator.SetLayerWeight(2, 1f);
    //            Debug.Log("OnStateUpdate");
    //            break;
    //        case 2:
    //            animator.SetLayerWeight(2, 0f);
    //            break;
    //        case 3:
    //            break;
    //    }
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    switch (Kind)
    //    {
    //        ////case 1:
    //        ////    animator.SetLayerWeight(2, 1f);
    //        ////    Debug.LogError("OnStateExit");
    //        ////    break;
    //        ////case 2:
    //        ////    //animator.SetLayerWeight(2, 0f);
    //        ////    break;
    //        //case 3:
    //        //    if(normalMovement !=null && normalMovement.IsDefense)
    //        //    {
    //        //        animator.SetLayerWeight(2, 1f);
    //        //        Debug.Log(stateInfo.IsName("GhostSamurai_APose2DefenseL_Root"));
    //        //    }
    //        //    else
    //        //    {
    //        //        animator.SetLayerWeight(2, 0f);

    //        //    }
    //        //    break;
    //    }
   
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
