using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PowerUpSystem : NetworkBehaviour
{
    public float attack;

    private CustomLobby netManager;
    //private NetworkVehicle net;
    // Start is called before the first frame update
    void Start()
    {
        netManager = GameObject.Find("LobbyManager").GetComponent<CustomLobby>();
        //net = GetComponent<NetworkVehicle>();
        //net.spawnBullet = new GameObject("SpawnBullet");
    }

    private void OnTriggerEnter(Collider other)
    {
        print("TRIGGERED");
        if (isLocalPlayer)
        {
            BoxPowerUp box = other.GetComponent<BoxPowerUp>();
            if (box != null)
            {
                int powerUpIndex = GetRandomPowerUpIndex();
                print(powerUpIndex);
                CmdTakePowerUp(other.gameObject, powerUpIndex);
            }
        }
    }


    private int GetRandomPowerUpIndex()
    {
        return UnityEngine.Random.Range(0, netManager.powerUpPrefabs.Count);
    }

    //Comandi e RPC
    [Command]
    public void CmdTakePowerUp(GameObject collider, int powerUpIndex)
    {
        if (!isClient)
        {
            PowerUp powerUp = netManager.powerUpPrefabs[powerUpIndex].GetComponent<PowerUp>();
            NetworkVehicle net = GetComponent<NetworkVehicle>();
            if (!net.IsPowerUpFull()) net.AddPowerUp(powerUp);
        }
        RpcTakePowerUp(collider, powerUpIndex);
        Destroy(collider);
    }

    [ClientRpc]
    public void RpcTakePowerUp(GameObject collider, int powerUpIndex)
    {
        PowerUp powerUp = netManager.powerUpPrefabs[powerUpIndex].GetComponent<PowerUp>();
        NetworkVehicle net = GetComponent<NetworkVehicle>();
        if (!net.IsPowerUpFull()) net.AddPowerUp(powerUp);
        Destroy(collider);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            //MoveCommands();
            PowerUpCommands();
            ShootCommands();
        }
        else if (isServer && !isClient)
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null) canvas.SetActive(false);
        }
    }

    [Command]
    public void CmdUsePowerUp(int index, Vector3 force)
    {
        List<PowerUp> ups = GetComponent<NetworkVehicle>().powerUps;
        if (!isClient && ups.Count > 0)
        {
            print("CMD USE");
            print("INDEX - TOT: " + index + "-" + GetComponent<NetworkVehicle>().powerUps.Count);
            PowerUp powerUp = Instantiate<PowerUp>(GetComponent<NetworkVehicle>().powerUps[index]);
            GetComponent<NetworkVehicle>().powerUps.RemoveAt(index);
            NetworkVehicle net = GetComponent<NetworkVehicle>();
            powerUp.transform.position = net.spawnBullet.transform.position;
            CoroutineData data = new CoroutineData(powerUp, force);
            //StartCoroutine("UsePowerUp", data);
            UsePowerUp(data);

        }
        else print("UPS EMPTY");
        RpcUsePowerUp(index, force);
    }

    [ClientRpc]
    public void RpcUsePowerUp(int index, Vector3 force)
    {
        List<PowerUp> ups = GetComponent<NetworkVehicle>().powerUps;
        if (ups.Count > 0)
        {
            print("RPC USE");
            NetworkVehicle net = GetComponent<NetworkVehicle>();
            print("INDEX - TOT: " + index + "-" + GetComponent<NetworkVehicle>().powerUps.Count);
            PowerUp powerUp = Instantiate<PowerUp>(GetComponent<NetworkVehicle>().powerUps[index]);
            powerUp.transform.position = net.spawnBullet.transform.position;
            CoroutineData data = new CoroutineData(powerUp, force);
            //StartCoroutine("UsePowerUp", data);
            UsePowerUp(data);
            GetComponent<NetworkVehicle>().powerUps.RemoveAt(index);
        }
        else print("UPS EMPTY");
    }

    [Command]
    public void CmdShootBullet(Vector3 trajectory)
    {
        if (!isClient)
        {
            Bullet bullet = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Bullet")).GetComponent<Bullet>();
            Vector3 position = GetComponent<NetworkVehicle>().spawnBullet.transform.position;
            bullet.transform.position = position;
            float cos = Vector3.Dot(Vector3.forward, trajectory.normalized);
            float sin = Vector3.Cross(Vector3.forward, trajectory.normalized).magnitude;
            double angle = Mathf.Atan2(cos, sin);
            //bullet.transform.Rotate(0, 0, Math.Round(angle));
            Rigidbody rigid = bullet.GetComponent<Rigidbody>();
            rigid.AddForce(bullet.speed * trajectory * 10);
            StartCoroutine("BulletTime");
        }
        RpcShootBullet(trajectory);
    }

    [ClientRpc]
    public void RpcShootBullet(Vector3 trajectory)
    {
        GameObject bullet = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Bullet"));
        bullet.GetComponent<Bullet>().attack = attack;
        /*Vector3 position = GetComponent<NetworkVehicle>().spawnBullet.transform.position;
        position.y -= 1;
        bullet.transform.position = position;*/
        //StartCoroutine("BulletTime");
        Vector3 position = GetComponent<NetworkVehicle>().spawnBullet.transform.position;
        bullet.transform.position = position;
        float cos = Vector3.Dot(Vector3.forward, trajectory.normalized);
        float sin = Vector3.Cross(Vector3.forward, trajectory.normalized).magnitude;
        double angle = Mathf.Atan2(cos, sin);
        //bullet.transform.Rotate(0, 0, Math.Round(angle));
        Rigidbody rigid = bullet.GetComponent<Rigidbody>();
        rigid.AddForce(bullet.GetComponent<Bullet>().speed * trajectory * 100);
        StartCoroutine("BulletTime", bullet);
    }

    //Funzioni per coroutines

    private void UsePowerUp(CoroutineData data)
    {
        //CoroutineData data = obj as CoroutineData;
        if (data != null)
        {
            PowerUp powerUp = data.powerUp;
            Vector3 force = data.force;
            NetworkVehicle net = GetComponent<NetworkVehicle>();
            powerUp.use(net, force);
            //yield return null;
        }
    }

    private IEnumerator BulletTime(object obj)
    {
        GameObject bullet = obj as GameObject;
        yield return new WaitForSeconds(20);
        Destroy(bullet.gameObject);
    }

    private void PowerUpCommands()
    {
        NetworkVehicle net = GetComponent<NetworkVehicle>();
        int powerUpIndex = 0;
        if (Input.GetKey(KeyCode.Alpha1) && net.powerUps.Count >= 1) //Selezione power up: Tasti 1234 (quelli sopra WASD)
        {
            powerUpIndex = 0;
        }
        else if (Input.GetKey(KeyCode.Alpha2) && net.powerUps.Count >= 2)
        {
            powerUpIndex = 1;
        }
        else if (Input.GetKey(KeyCode.Alpha3) && net.powerUps.Count >= 3)
        {
            powerUpIndex = 2;
        }
        else if (Input.GetKey(KeyCode.Alpha4) && net.powerUps.Count == 4)
        {
            powerUpIndex = 3;
        }
        if (Input.GetKey(KeyCode.Mouse1) && net.powerUps.Count >= 1) //Il click destro del mouse permette di utilizzare il power up scelto;
        {
            Vector2 force = calcForce();
            CmdUsePowerUp(powerUpIndex, force);
        }
    }

    private Vector3 calcForce()
    {
        Vector3 axe = calcOrientationAxe();
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        print("CENTER: " + center);
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseVector = mousePos - center;
        //print(mouseVector);
        print("MOUSE: " + mouseVector);
        print(Input.mousePosition);
        Vector3 throwVector = (axe.normalized + mouseVector.normalized).normalized * mouseVector.magnitude * 10;

        //print("THROW: " + throwVector);
        //Vector3 throwVector = axe*Input.mousePosition.magnitude*10;
        //print(throwVector);
        return throwVector;
    }

    private Vector3 calcOrientationAxe()
    {
        NetworkVehicle net = GetComponent<NetworkVehicle>();
        Vector3 spawnPos = net.spawnBullet.transform.position;
        Vector3 netPos = net.transform.position;
        Vector3 axe = new Vector3(spawnPos.x - netPos.x, 0, spawnPos.z - netPos.z);
        return axe;
    }

    private void ShootCommands()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 trajectory = calcForce().normalized;
            CmdShootBullet(trajectory);
        }
    }
}
