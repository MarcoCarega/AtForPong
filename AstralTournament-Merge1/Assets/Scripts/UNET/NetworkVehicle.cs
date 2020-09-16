using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkVehicle : NetworkBehaviour
{
    public int cannon;
    public int armor;
    public int engine;
    public int wheel;
    public Type defenseType;

    private GameObject board;
    public static bool changed;
    private bool done;

    public int health;
    public int maxHealth;

    public List<PowerUp> powerUps;

    public Texture status;

    public GameObject spawnBullet;

    void Start()
    {
        powerUps = new List<PowerUp>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPowerUp(PowerUp powerUp)
    {
        powerUps.Add(powerUp);
    }

    public void UsePowerUp(int index)
    {
        PowerUp powerUp = powerUps[index];
        powerUps.RemoveAt(index);
    }

    public bool IsPowerUpFull()
    {
        return powerUps.Count == 4;
    }
}
