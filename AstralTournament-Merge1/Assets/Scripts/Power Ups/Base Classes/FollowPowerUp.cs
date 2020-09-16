using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class FollowPowerUp : PowerUp
{
    protected abstract void OnFollow(Transform transform);

    protected override void OnUsePowerUp(NetworkVehicle net,Vector3 force)
    {
        Transform target=null;
        List<GameObject> players = GameObject.FindGameObjectsWithTag("Player").ToList();
        foreach(GameObject player in players)
        {
            if(!player.Equals(net.gameObject))
            {
                if (target == null) target = player.transform;
                else
                {
                    float distanceTarget = Vector3.Distance(target.position, net.transform.position);
                    float newDistance= Vector3.Distance(player.transform.position, net.transform.position);
                    if (newDistance < distanceTarget)
                        target = player.transform;
                }
            }
        }
        OnFollow(target);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
