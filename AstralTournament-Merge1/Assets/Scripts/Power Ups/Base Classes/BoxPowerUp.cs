
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BoxPowerUp : MonoBehaviour
{
    private CustomLobby netManager;

    // Start is called before the first frame update
    void Start()
    {
        //netManager = GameObject.Find("NetworkManager").GetComponent<CustomManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAroundLocal(Vector3.up, Time.deltaTime);
    }

    private void OnDestroy()
    {
        if (transform.parent != null)
        {
            PowerUpsSpawner pus = transform.parent.GetComponent<PowerUpsSpawner>();
            if (pus != null) { pus.Start(); }
        }
    }

}
