
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMessage : MessageBase
{
    public short controllerId;
    public int prefabIndex;
    public int cannon;
    public int armor;
    public int engine;
    public int wheel;
    /*public short cannonId;
    public short armorId;
    public short engineId;
    public short wheelId;*/

    public PlayerMessage()
    {

    }

    public PlayerMessage(NetworkVehicle net)
    {
        //NetworkVehicle net = vehicle.GetComponent<NetworkVehicle>();
        /*cannon = net.cannon;
        armor = net.armor;
        engine = net.engine;
        wheel = net.wheel;*/
    }

}
