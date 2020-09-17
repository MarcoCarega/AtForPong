using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_walk : StateMachineBehaviour
{
    public float speed = 1f;
    public float attackRange = 50f;

    private List<GameObject> fruitsToVisit;
    private List<GameObject> fruitsVisited;
    private GameObject target_fruit;
    private Rigidbody rb;
    public Transform target_player;
    private Boss bossScript;

    private void Awake()
    {
        fruitsToVisit = new List<GameObject>(GameObject.FindGameObjectsWithTag("Fruit"));
        fruitsVisited = new List<GameObject>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossScript = animator.GetComponent<Boss>();

        if (bossScript.attacked)
        {
            //target_player = GameObject.FindGameObjectWithTag("Player");
            target_player = bossScript.target_to_attack;
            bossScript.Rpc_moveToTarget();
        }
        else
        {
            target_fruit = findNextFruit();
            bossScript.Rpc_moveToFruit(target_fruit.name);
        }

        rb = animator.GetComponent<Rigidbody>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 targetPos;

        if (bossScript.attacked)
        {
            //target_player = GameObject.FindGameObjectWithTag("Player");
            target_player = bossScript.target_to_attack;

            if (target_player != null)
            {
                bossScript.Rpc_moveToTarget();

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
            else
            {
                targetPos = new Vector3(target_fruit.transform.position.x, rb.position.y, target_fruit.transform.position.z);

                target_fruit = findNextFruit();
                bossScript.Rpc_moveToFruit(target_fruit.name);

                if (Vector3.Distance(rb.position, targetPos) <= 5f)
                {
                    fruitsToVisit.Remove(target_fruit);
                    fruitsVisited.Add(target_fruit);
                    animator.SetTrigger("FruitReached");
                }

                bossScript.attacked = false;
            }

        }
        else
        {
            targetPos = new Vector3(target_fruit.transform.position.x, rb.position.y, target_fruit.transform.position.z);

            if (Vector3.Distance(rb.position, targetPos) <= 5f)
            {
                fruitsToVisit.Remove(target_fruit);
                fruitsVisited.Add(target_fruit);
                animator.SetTrigger("FruitReached");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (target_player == null)
        {
            animator.ResetTrigger("FruitReached");
        }
        else
        {
            animator.ResetTrigger("Attack_swing");
            animator.ResetTrigger("Attack_kick");
        }
    }

    private GameObject findNextFruit()
    {
        if (fruitsToVisit.Count == 0)
        {
            Debug.Log("Reset \"fruits to visit\" list");
            fruitsToVisit = new List<GameObject>(fruitsVisited);
            fruitsVisited = new List<GameObject>();
            target_fruit = fruitsToVisit[Random.Range(0, fruitsToVisit.Count - 1)];
        }
        else
        {
            target_fruit = fruitsToVisit[Random.Range(0, fruitsToVisit.Count - 1)];
        }

        return target_fruit;
    }
}
