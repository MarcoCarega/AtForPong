using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AstralArrow : FollowPowerUp
{
    public Transform destination;
    public float recalcTime;
    public int damage;
    public float speed;
    private Vector3 axe;

    private bool end;

    // Start is called before the first frame update
    void Start()
    {
        end = false;
        StartCoroutine("Chase");
        StartCoroutine("Destroy");
        axe = Vector3.right;
    }

    // Update is called once per frame
    void Update()
    {
        if (destination != null)
        {
            transform.LookAt(destination);
            axe = (destination.position - transform.position).normalized;
            Rigidbody rigid = GetComponent<Rigidbody>();
            rigid.AddForce(axe * speed);
        }
       // print(axe);
    }

    private IEnumerator Chase()
    {
       
        yield return new WaitForSeconds(recalcTime);
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(30);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        NetworkVehicle net=other.GetComponent<NetworkVehicle>();
        if(net!=null)
        {
            net.health -= damage;      
        }
        Destroy(gameObject);
    }

    protected override void OnFollow(Transform target)
    {
        destination = target;
    }
}
