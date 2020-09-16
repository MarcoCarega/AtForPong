
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetupSpawnable : MonoBehaviour
{
    private CustomLobby netManager;
    // Start is called before the first frame update
    void Start()
    {
        netManager = GameObject.Find("LobbyManager").GetComponent<CustomLobby>();
        netManager.componentPrefabs= Resources.LoadAll<GameObject>("Prefabs/Components").ToList();
        netManager.componentPrefabs.Insert(0, Resources.Load<GameObject>("Prefabs/UNET/Empty"));
        netManager.powerUpPrefabs = Resources.LoadAll<GameObject>("Prefabs/Power Ups").ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
