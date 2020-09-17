using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class Homuncolo : NetworkBehaviour
{
    public Transform target;
    public Transform foot;

    public LayerMask layerMask;

    private void Start()
    {
        target = Global.Instance.player.transform;
    }

    /*private void Update()
    {
        //print("Target: " + target);
        if(target==null)
        {
            List<GameObject> players = GameObject.FindGameObjectsWithTag("Player").ToList();
            target = players[Random.Range(0, players.Count - 1)].transform;
        }
    }*/

    [ClientRpc]
    public void Rpc_moveToTarget()
    {
        if (target != null)
        {
            GetComponent<NavMeshAgent>().destination = target.position;
        }
    }

    [ClientRpc]
    public void Rpc_Kick_Attack()
    {
        Vector3 scaleBox = new Vector3(.5f, .2f, .1f);

        Collider[] hitColliders = Physics.OverlapBox(foot.position, scaleBox, Quaternion.identity, layerMask);
        foreach (Collider col in hitColliders)
        {
            if (col.tag == "Player")
            {
                //col.GetComponent<Health>().CmdDealDamageWithAmount(attackDamage);
                Debug.Log("attacked " + col.name);
            }
        }
    }

}

/*public class Homuncolo : MonoBehaviour
{
    private Transform destination;
    private FSM fsm;

    // Start is called before the first frame update
    void Start()
    {
        destination = nearestPlayer();
        setupFSM();
    }

    // Update is called once per frame
    void Update()
    {
        if(destination==null)
        {
            destination = nearestPlayer();
        }
    }

    private Transform nearestPlayer()
    {
        List<GameObject> players = GameObject.FindGameObjectsWithTag("Player").ToList();
        Transform nearest= null;
        foreach(GameObject player in players)
        {
            if (nearest == null) nearest = player.transform;
            else
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                float distanceToNearest= Vector3.Distance(transform.position, nearest.position);
                if (distanceToPlayer < distanceToNearest)
                    nearest = player.transform;
            }
        }
        return nearest;
    }

    private void setupFSM()
    {
        FSMState initialState = makeStateMachine();
        fsm = new FSM(initialState);
    }

    private FSMState makeStateMachine()
    {
        List<FSMAction> actions=new List<FSMAction>();
        FSMState moveToPlayer = new FSMState();
        FSMState attackPlayer = new FSMState();
        moveToPlayer.AddTransition(new FSMTransition(isPlayerNear, actions), attackPlayer);
        moveToPlayer.AddTransition(new FSMTransition(isPlayerFar, actions), moveToPlayer);
        return moveToPlayer;
    }

    private bool isPlayerNear()
    {
        return destination != null && (Vector3.Distance(destination.position, transform.position) <= 1);
    }

    private bool isPlayerFar()
    {
        return destination != null && !isPlayerNear();
    }
}*/
