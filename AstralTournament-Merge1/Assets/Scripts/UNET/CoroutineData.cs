

using UnityEngine;

public class CoroutineData
{
    public PowerUp powerUp;
    public Vector3 force;

    public CoroutineData(PowerUp powerUp,Vector3 force)
    {
        this.powerUp = powerUp;
        this.force = force;
    }

    public string ToString()
    {
        return powerUp + " " + force;
    }
}