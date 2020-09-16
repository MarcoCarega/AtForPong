using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusPowerUp : PowerUp
{
    //private NetworkVehicle player;
    //private GameObject status;

    protected abstract void OnStatus();

    protected override void OnUsePowerUp(NetworkVehicle net,Vector3 force)
    {
        OnStatus();
    }

    // Start is called before the first frame update
    void Start()
    {
        //player = Global.Instance.player;
        //gameUI=GameObject.Find("Canvas").GetComponent<Ga>
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
