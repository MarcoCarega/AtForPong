using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelected : MonoBehaviour
{
    private Global global;
    public Character character;
    public Button button;
    

    private Vector3 nextPos;

    private bool done;

    // Start is called before the first frame update
    void Start()
    {  

        //nextPos = next.transform.position;
        //next.transform.position = new Vector3(nextPos.x + 50, nextPos.y, nextPos.z);
        global = Global.Instance;
        button.onClick.AddListener(onCLick);
        //next.onClick.AddListener(goNext);
    }

    private void onCLick()
    {
        global.character = character.name;
        NetworkVehicle net = Resources.Load<GameObject>("Prefabs/UNET/NetVehicle").GetComponent<NetworkVehicle>();
        net.defenseType = character.type;
    }

    // Update is called once per frame
    void Update()
    {
        if(global.hasCharacter() && !done)
        {
            
            done = true;
        }
    }

    private void goNext()
    {
        SceneManager.LoadScene("BuildVehicle");
    }
}
