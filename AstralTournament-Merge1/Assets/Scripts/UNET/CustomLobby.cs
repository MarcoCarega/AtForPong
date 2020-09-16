using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomLobby : NetworkLobbyManager
{
    public List<GameObject> componentPrefabs;
    public List<GameObject> powerUpPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSpawnPowerUpBox(int prefabIndex, Transform parent)
    {
        GameObject powerUp = Instantiate(spawnPrefabs[prefabIndex], parent.position + (Vector3.up * 1.5f), parent.rotation * Quaternion.AngleAxis(45f, Vector3.right), parent);
        NetworkServer.Spawn(powerUp);
    }
}
