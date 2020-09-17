using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class PowerUp : MonoBehaviour
{
    public Texture image;
    internal Transform thrower;

    protected abstract void OnUsePowerUp(NetworkVehicle net,Vector3 force);

    public void use(NetworkVehicle net, Vector3 force)
    {
        OnUsePowerUp(net,force);
    }

    public void setThrower(NetworkInstanceId target)
    {
        thrower = NetworkServer.FindLocalObject(target).transform;
    }
}
