
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NewPlayerController : NetworkBehaviour
{
    private CustomLobby netManager;
    private Global global;

    [SyncVar] public int cannon; //Componenti
    [SyncVar] public int armor;
    [SyncVar] public int engine;
    [SyncVar] public int wheel;
    [SyncVar] public Type type;

    //public GameObject vehiclePrefab;

    public float speed; //Statistiche
    public float acceleration;
    public float attack;
    public float defense;
    public float maneuverability;

    private List<WheelCollider> wheelColliders;
    private List<WheelCollider> steerWheels;
    private List<GameObject> wheels;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            wheelColliders = new List<WheelCollider>();
            steerWheels = new List<WheelCollider>();
            wheels = new List<GameObject>();
            global = Global.Instance;
            NetworkVehicle net = GetComponent<NetworkVehicle>();
            netManager = GameObject.Find("LobbyManager").GetComponent<CustomLobby>();
            if (isLocalPlayer)
            {
                GetComponent<CarMovements>().enabled = true;
                
                //GetComponent<InputManager>().enabled = true;
                //GetComponent<MoveCommands>().enabled = true;
                if(isServer) GetComponent<NetworkIdentity>().localPlayerAuthority = true;
                name = "LocalVehicle";
                CmdSetComponents(net.cannon, net.armor, net.engine, net.wheel, net.defenseType);
                global.player = GetComponent<NetworkVehicle>();
            }
            else if (net.cannon != 0 && net.armor != 0 && net.engine != 0 && net.wheel != 0 && !hasAuthority)
            {
                createVehicle();
            }
        }
        catch(Exception e)
        {
            print(e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount!=0) GetComponent<CannonCommands>().enabled = true;
        if (isLocalPlayer)
        {
           
            PowerUpCommands();
            ShootCommands();
        }     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isLocalPlayer)
        {
            BoxPowerUp box = other.GetComponent<BoxPowerUp>();
            if (box != null)
            {
                int powerUpIndex = GetRandomPowerUpIndex();
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


    [Command]
    public void CmdSetComponents(int cannon, int armor, int engine, int wheel, Type type)
    {
        this.cannon = cannon;
        this.wheel = wheel;
        this.engine = engine;
        this.armor = armor;
        this.type = type;
        if (isLocalPlayer || !isClient) createVehicle();
        RpcSetComponents(cannon, armor, engine, wheel, type);
    }

    [ClientRpc]
    public void RpcSetComponents(int cannon, int armor, int engine, int wheel, Type type)
    {
        this.cannon = cannon;
        this.wheel = wheel;
        this.engine = engine;
        this.armor = armor;
        this.type = type;
        createVehicle();
    }

    [Command]
    public void CmdUsePowerUp(int index, Vector3 force)
    {
        List<PowerUp> ups = GetComponent<NetworkVehicle>().powerUps;
        if (!isClient && ups.Count > 0)
        {
            PowerUp powerUp = Instantiate<PowerUp>(GetComponent<NetworkVehicle>().powerUps[index]);
            GetComponent<NetworkVehicle>().powerUps.RemoveAt(index);
            NetworkVehicle net = GetComponent<NetworkVehicle>();
            powerUp.transform.position = net.spawnBullet.transform.position;
            CoroutineData data = new CoroutineData(powerUp, force);
            //StartCoroutine("UsePowerUp", data);
            UsePowerUp(data);

        }
        RpcUsePowerUp(index, force);
    }

    [ClientRpc]
    public void RpcUsePowerUp(int index, Vector3 force)
    {
        List<PowerUp> ups = GetComponent<NetworkVehicle>().powerUps;
        if(ups.Count>0)
        {
            NetworkVehicle net = GetComponent<NetworkVehicle>();
            PowerUp powerUp = Instantiate<PowerUp>(GetComponent<NetworkVehicle>().powerUps[index]);
            powerUp.transform.position = net.spawnBullet.transform.position;
            CoroutineData data = new CoroutineData(powerUp, force);
            //StartCoroutine("UsePowerUp", data);
            UsePowerUp(data);
            GetComponent<NetworkVehicle>().powerUps.RemoveAt(index);
        }
    }

    [Command]
    public void CmdShootBullet(Vector3 trajectory)
    {
        if(!isClient)
        {
            Bullet bullet = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Bullet")).GetComponent<Bullet>();
            bullet.setShooter(netId);
            Vector3 position = GetComponent<NetworkVehicle>().spawnBullet.transform.position;
            bullet.transform.position = position;
            float cos = Vector3.Dot(Vector3.forward, trajectory.normalized);
            float sin = Vector3.Cross(Vector3.forward, trajectory.normalized).magnitude;
            double angle = Math.Atan2(cos, sin);
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
        bullet.GetComponent<Bullet>().setShooter(netId);
        Vector3 position = GetComponent<NetworkVehicle>().spawnBullet.transform.position;
        bullet.transform.position = position;
        float cos = Vector3.Dot(Vector3.forward, trajectory.normalized);
        float sin = Vector3.Cross(Vector3.forward, trajectory.normalized).magnitude;
        double angle = Math.Atan2(cos, sin);
        Rigidbody rigid = bullet.GetComponent<Rigidbody>();
        rigid.AddForce(bullet.GetComponent<Bullet>().speed * trajectory * 100);
        StartCoroutine("BulletTime",bullet);
    }

    //Funzioni per coroutines

    private void UsePowerUp(CoroutineData data)
    {
        if(data!=null)
        {
            PowerUp powerUp = data.powerUp;
            powerUp.setThrower(netId);
            Vector3 force = data.force;
            NetworkVehicle net = GetComponent<NetworkVehicle>();
            powerUp.use(net, force);
        }
    }

    private IEnumerator BulletTime(object obj)
    {
        GameObject bullet = obj as GameObject;
        yield return new WaitForSeconds(20);
        Destroy(bullet.gameObject);
    }

    //Funzioni supporto

    private void createVehicle()
    {
        if (transform.childCount != 0) return;
        try
        {
            netManager = GameObject.Find("LobbyManager").GetComponent<CustomLobby>();
            List<GameObject> prefab = netManager.componentPrefabs;
            Dictionary<string, VehicleComponent> set = new Dictionary<string, VehicleComponent>();
            set.Add("wheel", prefab[wheel].GetComponent<VehicleComponent>());
            set.Add("cannon", prefab[cannon].GetComponent<VehicleComponent>());
            set.Add("armor", prefab[armor].GetComponent<VehicleComponent>());
            set.Add("engine", prefab[engine].GetComponent<VehicleComponent>());
            setStats(set);
            GameObject vehicle = buildVehicle(set);
            print(vehicle);
            vehicle.transform.SetParent(transform, false);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private IEnumerator DestroyReference()
    {
        yield return new WaitForEndOfFrame();
        Destroy(GameObject.Find("Reference"));
        Destroy(GameObject.Find("SpawnBullet"));
    }

    public GameObject buildVehicle(Dictionary<string, VehicleComponent> set)
    {
        wheelColliders = new List<WheelCollider>();
        steerWheels = new List<WheelCollider>();
        wheels = new List<GameObject>();
        GameObject reference = new GameObject("Reference");
        GameObject board = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/TemplateComps/Board"), reference.transform);
        VehicleComponent cannon = Instantiate<VehicleComponent>(set["cannon"], reference.transform);
        if (cannon.name.Equals("Empty(Clone)"))
        {
            Destroy(reference);
            throw new Exception("creazione veicolo in più fermata");
        }
        GameObject spawn = new GameObject("SpawnBullet");
        print(spawn);
        GameObject cane = getCannonCane(cannon);
        spawn.transform.SetParent(cane.transform);
        spawn.transform.position = new Vector3(cane.transform.position.x + 0.03f, cane.transform.position.y - 0.06f, cane.transform.position.z + 4.25f);
        GetComponent<NetworkVehicle>().spawnBullet = spawn;
        VehicleComponent armor = Instantiate<VehicleComponent>(set["armor"], reference.transform);
        VehicleComponent engine = Instantiate<VehicleComponent>(set["engine"], reference.transform);
        GameObject center = new GameObject("Center");
        center.transform.position = transform.position + board.transform.position + cannon.transform.position + armor.transform.position + engine.transform.position;
        for (int i = 0; i < 4; i++)
        {
            VehicleComponent wheel = buildWheel(set["wheel"], i, reference.transform);
            wheel.name = wheel.name.Substring(0, wheel.name.Length - 7)+i;
            wheels.Add(wheel.gameObject);
            wheelColliders.Add(wheel.GetComponent<WheelCollider>());
            if (i<=1) steerWheels.Add(wheel.GetComponent<WheelCollider>());
            center.transform.position += wheel.transform.position;

            WheelCollider collider = wheel.GetComponent<WheelCollider>();
            collider.radius = 0.63f;
            collider.center = new Vector3(0, 0, 0);
        }
        center.transform.position /= 9;
        center.transform.SetParent(transform);
        SetupCollider();
        CarMovements car = GetComponent<CarMovements>();
        car.wheels = wheelColliders;
        car.meshes = wheels;
        car.steeringWheels = steerWheels;
        return reference;

    }

    private GameObject getCannonCane(VehicleComponent cannon)
    {
        return cannon.transform.GetChild(0).gameObject;
    }

    private void SetupCollider()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.size = new Vector3(6.2940347f,2.506618f,6.808867f);
        collider.center = new Vector3(-0.05883098f,0.4f,-0.4374144f);
    }

    private VehicleComponent buildWheel(VehicleComponent wheel, int i, Transform parent)
    {

        VehicleComponent buildwheel = Instantiate<VehicleComponent>(wheel, parent);
        if (i==0)
        {
            //top left;
            buildwheel.transform.position = new Vector3(-2.463034f, 0.0188747f, 1.956946f);
        }
        else if (i == 1)
        {//top right;
            buildwheel.transform.position = new Vector3(2.463034f, 0.0188747f, 1.956946f);
            buildwheel.transform.Rotate(0, 0, -180);
        }
        else if (i == 2 )
        {//bottom left
            buildwheel.transform.position = new Vector3(-2.463034f, 0.0188747f, -2.8425256f);
        }
        else if (i == 3)
        { //bottom right
            buildwheel.transform.position = new Vector3(2.463034f, 0.0188747f, -2.8425256f);
            buildwheel.transform.Rotate(0, 0, -180);
        }

        return buildwheel;
    }

    private void setupRigidBody()
    {
        Rigidbody rigid = gameObject.GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
    }

    private void setStats(Dictionary<string,VehicleComponent> set)
    {
        speed = set["engine"].values[0];
        acceleration = set["engine"].values[1];
        attack = set["cannon"].values[0];
        defense = set["armor"].values[0];
        maneuverability = set["armor"].values[1] + set["wheel"].values[0];
    }

    private void PowerUpCommands()
    {
        NetworkVehicle net = GetComponent<NetworkVehicle>();
        int powerUpIndex = 0;
        if(Input.GetKeyDown(KeyCode.Alpha1) && net.powerUps.Count>=1) //Selezione power up: Tasti 1234 (quelli sopra WASD)
        {
            powerUpIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && net.powerUps.Count >= 2)
        {
            powerUpIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && net.powerUps.Count >= 3)
        {
            powerUpIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && net.powerUps.Count == 4)
        {
            powerUpIndex = 3;
        }
        if(Input.GetKeyDown(KeyCode.Mouse1) && net.powerUps.Count >= 1) //Il click destro del mouse permette di utilizzare il power up scelto;
        {
            Vector2 force = calcForce();
            CmdUsePowerUp(powerUpIndex,force);
        }
    }

    private Vector3 calcForce()
    {
        NetworkVehicle net = GetComponent<NetworkVehicle>();
        if (net.spawnBullet == null) return Vector3.one;
        CustomLobby lobby = GameObject.Find("LobbyManager").GetComponent<CustomLobby>();
        GameObject cane = GameObject.Find("LocalVehicle/Reference/" + lobby.componentPrefabs[net.cannon].name + "(Clone)/CannonCane");
        Vector3 axe = net.spawnBullet.transform.position - cane.transform.position;
        return axe.normalized * Input.mousePosition.magnitude*5;
    }

    private Vector3 calcOrientationAxe()
    {
        NetworkVehicle net = GetComponent<NetworkVehicle>();
        Vector3 spawnPos = net.spawnBullet.transform.position;
        Vector3 netPos = net.transform.position;
        Vector3 axe = new Vector3(spawnPos.x-netPos.x,0,spawnPos.z - netPos.z);
        return axe;
    }

    private void ShootCommands()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 trajectory = calcForce().normalized;
            CmdShootBullet(trajectory);
        }
    }

}
