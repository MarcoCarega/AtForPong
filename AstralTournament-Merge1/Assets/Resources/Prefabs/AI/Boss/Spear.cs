using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    //public Transform handToFollow;
    public int attackDamage = 20;
    public float attackRange = 0.1f;
    public Transform spearTip;

    public LayerMask layerMask;

    private bool m_Started;
    private bool attacking;
    //private Boss bossScript;

    // Start is called before the first frame update
    void Start()
    {
        //transform.up = handToFollow.forward;
        m_Started = true;
        //bossScript = GetComponent<Boss>();
    }

    public void Spear_Attack()
    {
        Vector3 scaleBox = new Vector3(2f,.3f,.2f);

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

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Vector3 scaleBox = new Vector3(2f, .3f, .2f);

        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(spearTip.position, scaleBox);
    }
}
