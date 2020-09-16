using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class SetupNetwork : MonoBehaviour
{
    private Global global;
    private CustomLobby netManager;
    // Start is called before the first frame update
    void Start()
    {
        global = Global.Instance;
        GameObject prefab = global.networkVehicle;
        netManager = GameObject.Find("LobbyManager").GetComponent<CustomLobby>();
        prefab.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
