using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Classe che permette di abilitare il bottone per andare alla selezione arena
public class GoToArenaUI : MonoBehaviour
{

    //public Vehicle astroMachine;
    private Global global;
    public Vehicle vehicle;
    // Start is called before the first frame update
    void Start()
    {
        global = Global.Instance;
        GetComponent<Button>().onClick.AddListener(onClick);
    }

    private void onClick()//quando cliccato si prepara il veicolo al networking (non più necessario dato che non si utilizza il network vehicle ora)
    {//dopo si salva il veicolo in global e si passa alla scena della selezione arena



        //astroMachine.transform.SetParent(null);
        //passStats(ref astroMachine);
        //global.addVehicle(astroMachine);
        //Vehicle vehicle = global.GetVehicle();
        //print(vehicle);
        //vehicle.blocked = true;
        //vehicle.transform.SetParent(null);
        //vehicle.gameObject.AddComponent<NetworkIdentity>().localPlayerAuthority = true;
        //vehicle.gameObject.AddComponent<NetworkTransform>().transformSyncMode=NetworkTransform.TransformSyncMode.SyncTransform;
        //GameObject game = PrefabUtility.SaveAsPrefabAssetAndConnect(vehicle.gameObject, "Assets/Resources/Prefabs/Astromachine.prefab", InteractionMode.UserAction);
        //global.addVehicle(vehicle);
        //global.GetVehicle().transform.SetParent(null);
        //DontDestroyOnLoad(global.GetVehicle());
        SceneManager.LoadScene("SelectArena");
        //DontDestroyOnLoad(game);
    }

    private void passStats(ref Vehicle game)
    {
        //Assemble game = astroMachine;//Instantiate(astroMachine) as Assemble;
        //game.transform.SetParent(null);
        /*game.attack = astroMachine.attack;
        game.defense = astroMachine.defense;
        game.speed = astroMachine.speed;
        game.acceleration = astroMachine.acceleration;
        game.maneuverability = astroMachine.maneuverability;*/
        game.blocked = true;
        //return game;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

}
