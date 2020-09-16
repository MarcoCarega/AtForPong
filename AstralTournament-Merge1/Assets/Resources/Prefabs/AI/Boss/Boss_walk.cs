using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_walk : StateMachineBehaviour
{
    public float speed = 1f;
    public float attackRange = .2f;

    private List<GameObject> fruits;
    private GameObject target_fruit;
    private Rigidbody rb;
    public GameObject target_player;
    //private bool targetReached = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<Boss>().attacked)
        {
            target_player = GameObject.FindGameObjectWithTag("Player");
            animator.GetComponent<Boss>().moveTo(target_player);
        }

        if (target_player == null)
        {
            fruits = new List<GameObject>(GameObject.FindGameObjectsWithTag("Fruit"));
            target_fruit = fruits[Random.Range(0, fruits.Count - 1)];
            animator.GetComponent<Boss>().moveTo(target_fruit);
            animator.GetComponent<Boss>().attacked = false;
        }

        rb = animator.GetComponent<Rigidbody>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 targetPos;

        if (target_player == null)
        {
            //animator.GetComponent<Boss>().moveToFruit();

            targetPos = new Vector3(target_fruit.transform.position.x, rb.position.y, target_fruit.transform.position.z);
            //rb.transform.LookAt(targetPos);
            animator.GetComponent<Boss>().moveTo(target_fruit);

            //Vector3 newPos = Vector3.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime);
            //rb.MovePosition(newPos);

            if (Vector3.Distance(rb.position, targetPos) <= attackRange)
            {
                
                animator.SetTrigger("FruitReached");
            }

        }
        else
        {
            //animator.GetComponent<Boss>().moveToTarget(new ChaseData(this,target_player.transform));

            animator.GetComponent<Boss>().moveTo(target_player);

            targetPos = new Vector3(target_player.transform.position.x, rb.position.y, target_player.transform.position.z);
            //rb.transform.LookAt(targetPos);

            //Vector3 newPos = Vector3.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime);
            //rb.MovePosition(newPos);

            if (Vector3.Distance(rb.position, targetPos) <= 20f)
            {
                int rand = Random.Range(1, 10);
                Debug.Log(rand);
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
        if (target_player == null)
        {
            animator.ResetTrigger("FruitReached");
            Destroy(target_fruit);
        }
        else
        {
            animator.ResetTrigger("Attack_swing");
            animator.ResetTrigger("Attack_kick");
        }
    }
}
