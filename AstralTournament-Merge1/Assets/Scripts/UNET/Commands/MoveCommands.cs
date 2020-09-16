using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MoveCommands : NetworkBehaviour
{

    public InputManager im;
    public List<WheelCollider> wheels;
    public List<WheelCollider> steeringWheels;
    public List<GameObject> meshes;
    public float torqueCoefficient = 200000f;
    public float maxTurn = 20f;
    public float brakeStrength;
    public Transform CM;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        im = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        if (!im.enabled) im.enabled = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isLocalPlayer)
        {
            foreach(WheelCollider wheelCol in wheels)
            {
                CmdMove(im.brake, wheelCol.gameObject);
            }
            CmdSteerWheels();
            CmdRotateWheels();
        }
        
    }

    [Command]
    public void CmdSteerWheels()
    {
        foreach (WheelCollider wheel in steeringWheels)
        {
            wheel.steerAngle = maxTurn * im.steer;
            float angle = 0;
            if (wheel.name[wheel.name.Length - 1] == '1') angle = 180;
            wheel.transform.localEulerAngles = new Vector3(0f, angle + im.steer * maxTurn, 90f); //il figlio del wheelcollider è il mesh della ruota che deve ruotare
        }
        RpcSteerWheels();
    }

    [ClientRpc]
    public void RpcSteerWheels()
    {
        foreach (WheelCollider wheel in steeringWheels)
        {
            wheel.steerAngle = maxTurn * im.steer;
            float angle = 0;
            if (wheel.name[wheel.name.Length - 1] == '1') angle = 180;
            wheel.transform.localEulerAngles = new Vector3(0f, angle + im.steer * maxTurn, 90f); //il figlio del wheelcollider è il mesh della ruota che deve ruotare
        }
    }

    [Command]
    public void CmdRotateWheels()
    {
        foreach (GameObject mesh in meshes)
        {
            int a = 1;
            if (mesh.name[mesh.name.Length - 1] == '2') a = -1;
            mesh.transform.Rotate(0f, a * rb.velocity.magnitude * (transform.InverseTransformDirection(rb.velocity).z >= 0 ? 1 : -1) / (2 * Mathf.PI * .2f), 0f);
        }
    }

    [ClientRpc]
    public void RpcRotateWheels()
    {
        foreach (GameObject mesh in meshes)
        {
            int a = 1;
            if (mesh.name[mesh.name.Length - 1] == '2') a = -1;
            mesh.transform.Rotate(0f, a * rb.velocity.magnitude * (transform.InverseTransformDirection(rb.velocity).z >= 0 ? 1 : -1) / (2 * Mathf.PI * .2f), 0f);
        }
    }

    [Command]
    public void CmdMove(bool brake,GameObject game)
    {
        if(game!=null)
        {
            WheelCollider wheel = game.GetComponent<WheelCollider>();
            if (brake) stopMove(wheel);
            else move(wheel);
            RpcMove(brake, game.name);
        }
    }

    [ClientRpc]
    public void RpcMove(bool brake,string gameName)
    {
        if(transform.childCount!=0)
        {
            GameObject game = null;
            for (int i = 0; i < transform.GetChild(1).childCount; i++)
                if (transform.GetChild(1).GetChild(i).gameObject.name.Equals(gameName))
                    game = transform.GetChild(1).GetChild(i).gameObject;
            WheelCollider wheel = game.GetComponent<WheelCollider>();
            if (brake) stopMove(wheel);
            else move(wheel);
        }
        
    }

    private void move(WheelCollider wheel)
    {
        print(wheel);
        if (wheel == null) return;
        wheel.motorTorque = torqueCoefficient * im.throttle;// * Time.deltaTime;                                                  //Debug.Log(im.throttle + "\n" + Time.deltaTime);
        wheel.brakeTorque = 0f;
    }

    private void stopMove(WheelCollider wheel)
    {
        wheel.brakeTorque = brakeStrength; // * Time.deltaTime;
        wheel.motorTorque = 0f;
    }

}
