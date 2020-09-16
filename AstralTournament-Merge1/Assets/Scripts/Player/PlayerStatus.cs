using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//classe indicante lo status del player
public class PlayerStatus : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() //se camera e veicolo sono presenti come figli del giocatore, abilita il movimento
    {
        if (transform.childCount != 2)
            GetComponent<VehicleMovement>().enabled = false;
        else GetComponent<VehicleMovement>().enabled = true;
    }
}
