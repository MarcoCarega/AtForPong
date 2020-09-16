using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PowerUpsSpawner : NetworkBehaviour
{
    //public List<GameObject> spawnables;

    //private GameObject powerUp;

    // Start is called before the first frame update
    public void Start()
    {
        StartCoroutine("SpawnPowerup");
    }

    [Command]
    public void CmdSpawnObj() {

        NetworkLobbyManager.singleton.GetComponent<CustomLobby>().OnSpawnPowerUpBox(0, transform);
        
    }

    private IEnumerator SpawnPowerup()
    {
        yield return new WaitForSeconds(10f);

        CmdSpawnObj();

        //powerUp = Instantiate(spawnables[Random.Range(0, spawnables.Count)], transform.position + (Vector3.up * 1.5f), transform.rotation * Quaternion.AngleAxis(45f, Vector3.right), transform);
        //NetworkServer.Spawn(powerUp);
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.collider.CompareTag("Player"))
        {
            Destroy(powerUp);
            NetworkServer.Destroy(powerUp);
            CmdSpawnObj();
        }
        if (powerUp != null)
        {
            Debug.Log("Power up taken");
            CmdSpawnObj();
        }

    }*/

    /*private void Update()
    {

        //if (!isLocalPlayer) { return;  }
        
        if (transform.childCount == 0)
        {
            StartCoroutine("SpawnPowerup");
        }

    }*/
}
