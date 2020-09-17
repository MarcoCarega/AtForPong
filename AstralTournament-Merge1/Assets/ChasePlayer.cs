using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : StateMachineBehaviour
{
    /*private NavMeshAgent agent;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        Homuncolo homuncolo = animator.GetComponent<Homuncolo>();
        if (homuncolo.target != null)
        {
            agent.destination = homuncolo.target.position;
        }
        if(Vector3.Distance(agent.destination,animator.transform.position)<=1)
        {
            animator.SetTrigger("AttackPlayer");
        }
    }*/

    public float speed = 8f;
    public float attackRange = 7f;

    private Rigidbody rb;
    public Transform target_player;
    private Homuncolo homuScript;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        homuScript = animator.GetComponent<Homuncolo>();
        rb = animator.GetComponent<Rigidbody>();
        
        target_player = homuScript.target;
        homuScript.Rpc_moveToTarget();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 targetPos;
        target_player = homuScript.target;

        if (target_player != null)
        {
            homuScript.Rpc_moveToTarget();

            targetPos = new Vector3(target_player.transform.position.x, rb.position.y, target_player.transform.position.z);

            if (Vector3.Distance(rb.position, targetPos) <= attackRange)
            {
                int rand = Random.Range(1, 10);
                //Debug.Log(rand);
                if (rand > 5)
                {
                    animator.SetTrigger("Attack_swing");
                }
                else
                {
                    animator.SetTrigger("Attack_kick");
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack_swing");
        animator.ResetTrigger("Attack_kick");
    }

}
