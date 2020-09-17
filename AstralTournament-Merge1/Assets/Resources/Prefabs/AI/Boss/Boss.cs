using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

[RequireComponent(typeof(NavMeshAgent))]
public class Boss : NetworkBehaviour
{
    public float health = 1000f;
    public bool attacked;
    public int kickDamage = 20;
    public int spearDamage = 30;
    public Transform spearTip;
    public Transform foot;

    public LayerMask layerMask;

    public Transform target_to_attack;

    private bool m_Started;

    // Start is called before the first frame update
    void Start()
    {
        //transform.up = handToFollow.forward;
        m_Started = true;
    }

    [ClientRpc]
    public void Rpc_Spear_Attack()
    {
        Vector3 scaleBox = new Vector3(2f, .3f, .2f);

        Collider[] hitColliders = Physics.OverlapBox(spearTip.position, scaleBox, Quaternion.identity, layerMask);
        foreach (Collider col in hitColliders)
        {
            if (col.tag == "Player")
            {
                //col.GetComponent<Health>().CmdDealDamageWithAmount(attackDamage);
                Debug.Log("attacked " + col.name);
            }
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

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Vector3 scaleBox1 = new Vector3(2f, .3f, .2f);
        Vector3 scaleBox2 = new Vector3(.5f, .2f, .1f);

        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
        {
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(spearTip.position, scaleBox1);
            Gizmos.DrawWireCube(foot.position, scaleBox2);
        }
    }

    public void takeDamage(float value, NetworkInstanceId netID)
    {
        //if (!attacked) attacked = true;
        Cmd_takeDamage(value, netID);
    }

    [Command]
    private void Cmd_takeDamage(float damage, NetworkInstanceId netID)
    {
        Rpc_takeDamage(damage, netID);
    }

    [ClientRpc]
    public void Rpc_takeDamage(float amount, NetworkInstanceId netID)
    {
        if (!attacked)
        {
            attacked = true;
            Debug.Log(netID);
            target_to_attack = NetworkServer.FindLocalObject(netID).transform;
        }
        health -= amount;
        Debug.Log("Boss Hitted, health decreased.");
        if (health <= 0)
        {
            Rpc_die();
        }
    }

    [ClientRpc]
    private void Rpc_die()
    {
        Destroy(gameObject);
        Debug.Log("Boss died");
    }

    [ClientRpc]
    public void Rpc_moveToFruit(string name)
    {

        Debug.Log("going to " + name);
        GetComponent<NavMeshAgent>().destination = GameObject.Find(name).transform.position;
    }

    [ClientRpc]
    public void Rpc_moveToTarget()
    {
        if (target_to_attack != null)
        {
            GetComponent<NavMeshAgent>().destination = target_to_attack.position;
        }
    }

}
