using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_attack : StateMachineBehaviour
{
    private Transform target_player;
    private Rigidbody rb;
        
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody>();
        target_player = animator.GetComponent<Boss>().target_to_attack;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (target_player != null)
        {
            if (Vector3.Distance(rb.position, target_player.position) > 10f)
            {
                animator.SetTrigger("Chase");
            }
        }
        else
        {
            animator.SetTrigger("Chase");
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Chase");
        //Debug.Log("exiting from attack state");
    }
}
