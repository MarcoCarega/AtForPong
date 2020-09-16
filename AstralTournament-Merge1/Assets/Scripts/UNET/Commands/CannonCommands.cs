using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CannonCommands : NetworkBehaviour
{
    public Transform tower;
    public Transform cannon;
    //public float towerSpeed;
    public float cannonSpeed;
    public GameObject crossHairGO;

    //private float towerAngle;
    private float cannonAngle;
    //private Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        crossHairGO = GameObject.Find("crosshair");
        cannon = null;
        tower = null;
    }

    // Update is called once per frame
    void Update()
    {
       if(transform.childCount!=0)
        {
            string cannonName = getCannonName();
            GameObject towerObj = transform.GetChild(1).GetChild(1).gameObject;
            GameObject cannonObj = towerObj.transform.GetChild(0).gameObject;
            if (cannonObj == null || towerObj == null) return;
            cannon = cannonObj.transform;
            tower = towerObj.transform;
            if (isLocalPlayer)
            {
                rotateTower();
                rotateCannon();

            }
            moveCrosshair();
        }
    }

    private string getCannonName()
    {
        CustomLobby lobby = GameObject.Find("LobbyManager").GetComponent<CustomLobby>();
        NetworkVehicle net = GetComponent<NetworkVehicle>();
        return lobby.componentPrefabs[net.cannon].name;
    }

    void moveCrosshair()
    {

        Vector3 chPos = Input.mousePosition;
        chPos.x = Mathf.Clamp(chPos.x, 0, Screen.width);
        chPos.y = Mathf.Clamp(chPos.y, 0, Screen.height);
        crossHairGO.GetComponent<RawImage>().transform.position = chPos;

    }

    void rotateTower()
    {
        //towerAngle += Input.GetAxis("Mouse X") * towerSpeed * -Time.deltaTime;
        //tower.localRotation = Quaternion.AngleAxis(-towerAngle, Vector3.up);
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (ground.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            CmdRotateTower(pointToLook);
        }

    }

    void rotateCannon()
    {

        cannonAngle += Input.GetAxis("Mouse Y") * cannonSpeed * -Time.deltaTime;
        cannonAngle = Mathf.Clamp(cannonAngle, 80f, 100f);
        CmdRotateCannon(cannonAngle);

    }

    [Command]
    public void CmdRotateTower(Vector3 pointToLook)
    {
        tower.LookAt(new Vector3(pointToLook.x, tower.position.y, pointToLook.z));
        RpcRotateTower(pointToLook);
    }

    [ClientRpc]
    public void RpcRotateTower(Vector3 pointToLook)
    {
        tower.LookAt(new Vector3(pointToLook.x, tower.position.y, pointToLook.z));
    }

    [Command]
    public void CmdRotateCannon(float cannonAngle)
    {
        if(cannon!=null)
        {
            cannon.localRotation = Quaternion.AngleAxis(cannonAngle, Vector3.right);
            RpcRotateCannon(cannon.transform.localRotation.eulerAngles);
        }
    }

    [ClientRpc]
    public void RpcRotateCannon(Vector3 rotation)
    {
        if (cannon != null)
            cannon.localRotation = Quaternion.Euler(rotation);
    }
}
