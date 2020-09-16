using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GreenPotion : StatusPowerUp
{
    public int defenseIncrease;
    public int timer;
    private NetworkVehicle player;
    private Material material;
    private Thread thread;
    protected override void OnStatus()
    {
        player = Global.Instance.player;
        material = GetComponent<Renderer>().sharedMaterial;
        StartCoroutine("StatusTime");
        transform.localScale = Vector3.zero;
    }

    private IEnumerator StatusTime()
    {   
        player.status = material.mainTexture;
        player.GetComponent<NewPlayerController>().defense += defenseIncrease;
        yield return new WaitForSeconds(timer);
        player.GetComponent<NewPlayerController>().defense -= defenseIncrease;
        player.status = null;
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Thread thread = new Thread(new ThreadStart(statusTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
