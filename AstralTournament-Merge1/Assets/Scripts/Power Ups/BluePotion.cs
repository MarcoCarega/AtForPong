using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePotion : StatusPowerUp
{
    public int healthIncrement;
    private NetworkVehicle player;

    protected override void OnStatus()
    {
        player = Global.Instance.player;
        int tmpHelth = player.health + healthIncrement;
        if (tmpHelth <= player.maxHealth) player.health = tmpHelth;
        else player.health = player.maxHealth;
        Destroy(gameObject);
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
