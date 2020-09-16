
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Classe che assembla il veicolo
public class Vehicle : MonoBehaviour
{
    /*public VehicleComponent cannon;
    public VehicleComponent armor;
    public VehicleComponent engine;
    public VehicleComponent wheel;*/

    /*public Select cannon;
    public Select armor;
    public Select engine;
    public Select wheel;*/

    private GameObject board;

    public bool changed;
    private bool first;
    private Global global;
    public GameObject statsView; //tabellone statistiche veicolo

    public float speed=0; //statistiche
    public float acceleration=0;
    public float attack=0;
    public float defense=0;
    public float maneuverability=0;
    public bool blocked; //se true, il veicolo non può più essere modificato
    private Dictionary<string, VehicleComponent> set; //dizionario con le componenti;

    private VehicleComponent cannon; // componenti
    private VehicleComponent armor;
    private VehicleComponent engine;
    private VehicleComponent wheel;

    private NetworkVehicle net; //variabile con i nomi delle componenti usate (deve essere poi inviato in rete)

    // Start is called before the first frame update
    void Start()
    {
        first = true;
        global = Global.Instance;
        GameObject game = GameObject.Find("NetVehicle(Clone)");
        if (game != null) net = game.GetComponent<NetworkVehicle>();
        else net =Resources.Load<GameObject>("Prefabs/UNET/NetVehicle").GetComponent<NetworkVehicle>();
        DontDestroyOnLoad(net);
        global.networkVehicle = net.gameObject;
        this.transform.Rotate(0, 230, 0);
        
        
        //global.networkVehicle = net;
        //print(net);
    }

    // Update is called once per frame
    void Update()
    {
        set = Select.getSet(); //Prende i componenti del veicolo dai selettori
        if (first) //Alla prima esecuzione di Update il veicolo non è ancora assemblato
        {
            Select.changed = true; //quindi, si setta questo valore a true per poter entrare nell'if che c'è dopo
            first = false;
        }
        if (Select.changed && !blocked && set.Count==4) //si entra in questo if solo se almeno un componente è cambiato e il veicolo non è bloccato
        {
            setComponentNet(set);
            //set = Select.getSet();
            Destroy(board);     //se è già presente un veicolo assemblato, questo viene distrutto e rimosso da Global
            //global.removeVehicle();

            //board=build(set); // costruzione effettiva
            board = buildVehicle(set);
            board.transform.SetParent(this.transform);
            board.transform.localScale = new Vector3(5, 5, 5);
            adjustVehicleForUI(); //prepara il veicolo costruito per essere mostrato nell'UI
            setStats(set); //aggiorna le statistiche nel tabellone dell'UI
            //global.addVehicle(GetComponent<Vehicle>());
            NetworkVehicle.changed = Select.changed; //se c'è stato un cambiamento, allora bisogna anche aggiornare le variabili delle componenti
            Select.changed = false;
            setComponentNet(set);
            /*if (net != null) Destroy(net); 
            net = createNetVehicle(); //risetta le variabili per le componenti utilizzate
            //net.GetComponent<NetworkVehicle>().create();
            DontDestroyOnLoad(net);
            global.networkVehicle = net;*/
            //net.transform.SetParent(transform);
        }
    }

    private void setComponentNet(Dictionary<string,VehicleComponent> set)
    {
        global.cannon=net.cannon = getSpawnIndex(set["cannon"]);
        global.armor=net.armor = getSpawnIndex(set["armor"]);
        global.engine=net.engine = getSpawnIndex(set["engine"]);
        global.wheel=net.wheel = getSpawnIndex(set["wheel"]);
    }

    private int getSpawnIndex(VehicleComponent comp)
    {
        bool end = false;
        int index = -1;
        CustomLobby netManager = GameObject.Find("LobbyManager").GetComponent<CustomLobby>();
        for (int i = 0; i < netManager.componentPrefabs.Count && !end; i++)
        {
            //print(comp + " -- " + netManager.spawnPrefabs[i]);
            if (comp.name.Equals(netManager.componentPrefabs[i].name))
            {
                index = i;
                end = true;
            }
        }
        return index;
    }

 

    public GameObject buildVehicle(Dictionary<string, VehicleComponent> set)
    {
        GameObject reference = new GameObject("Reference");
        GameObject board = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/TemplateComps/Board"), reference.transform);
        VehicleComponent cannon = Instantiate<VehicleComponent>(set["cannon"], reference.transform);
        VehicleComponent armor = Instantiate<VehicleComponent>(set["armor"], reference.transform);
        VehicleComponent engine = Instantiate<VehicleComponent>(set["engine"],reference.transform);

        for (int i=0; i<4; i++)
        {
            VehicleComponent wheel = buildWheel(set["wheel"], i, reference.transform);
             
        }

        return reference;
        
    }

    private VehicleComponent buildWheel(VehicleComponent wheel, int i, Transform parent)
    {
       
        VehicleComponent buildwheel = Instantiate<VehicleComponent>(wheel, parent);
         if( i ==   1 )
        {//top right;
            buildwheel.transform.position = new Vector3(2.45105f, -0.004784107f, 1.956946f);
            buildwheel.transform.Rotate(0, 0, -180);
        }
        else if( i == 2)
        {//bottom left
            buildwheel.transform.position = new Vector3(-2.463034f, 0.0188747f, -2.8425256f);
        }
        else if( i == 3 )
        { //bottom right
            buildwheel.transform.position = new Vector3(2.45105f, -0.004784107f, -2.863177f);
            buildwheel.transform.Rotate(0, 0, -180);
        }

        return buildwheel;
    }

    private GameObject createNetVehicle()
    {
        GameObject game;// = GameObject.Find("NetVehicle");
        //if (game != null) Destroy(game);
        game = new GameObject("NetVehicle");
        NetworkVehicle net = game.AddComponent<NetworkVehicle>(); //crea il gameobject e aggiunge la compoente NetworkVehicle
        net.cannon = getSpawnIndex(cannon);
        net.armor = getSpawnIndex(armor);
        net.engine = getSpawnIndex(engine);
        net.wheel = getSpawnIndex(wheel);
        //net.vehicle = this;
        return game;
    }

    private void adjustVehicleForUI()
    {
        //board.AddComponent<Vehicle>();
        //passStats(board.GetComponent<Vehicle>(), this);
        board.transform.position = transform.position;
        board.transform.rotation = transform.rotation;
        //board.transform.SetParent(transform);
       // board.transform.localScale = 3 * board.transform.localScale;
    }

    private void passStats(Vehicle game,Vehicle astroMachine)
    {
        //Assemble game = astroMachine;//Instantiate(astroMachine) as Assemble;
        //game.transform.SetParent(null);
        game.attack = astroMachine.attack;
        game.defense = astroMachine.defense;
        game.speed = astroMachine.speed;
        game.acceleration = astroMachine.acceleration;
        game.maneuverability = astroMachine.maneuverability;
        //game.block();
        //return game;
    }

    private VehicleComponent createCannon(GameObject board,VehicleComponent vehicleComponent)
    {
        VehicleComponent cannonInstance = copyObject(vehicleComponent);
        cannonInstance.transform.position = new Vector3(0, 0, 0);
        cannonInstance.transform.localScale = new Vector3(1, 1, 1);
        cannonInstance.transform.localScale = 2 * cannonInstance.transform.localScale;
        float y = 3.5f * board.transform.localScale.y + 2.5f;
        cannonInstance.transform.Translate(0, y, 0);
        return cannonInstance;
    }

    private VehicleComponent createArmor(VehicleComponent vehicleComponent)
    {
        VehicleComponent armorInstance = copyObject(vehicleComponent);
        armorInstance.transform.position = new Vector3(0, 0, 0);
        armorInstance.transform.localScale = new Vector3(6, 4, 6);
        armorInstance.transform.Translate(0, 0.5f + armorInstance.transform.localScale.y, 0);
        armorInstance.transform.localScale = armorInstance.transform.localScale / 4;
        armorInstance.transform.Rotate(0, 180, 0);
        return armorInstance;
    }

    private VehicleComponent createEngine(GameObject board,VehicleComponent vehicleComponent)
    {
        VehicleComponent engineInstance = copyObject(vehicleComponent);
        engineInstance.transform.position = new Vector3(0, 0, 0);
        engineInstance.transform.localScale = new Vector3(2, 2, 2);
        engineInstance.transform.Translate(0, engineInstance.transform.localScale.y / 2, -board.transform.localScale.z / 2);
        return engineInstance;
    }

    private VehicleComponent createWheel(GameObject board,VehicleComponent component, int index)
    {
        VehicleComponent wheelInstance=copyObject(component);
        wheelInstance.transform.position = new Vector3(0, 0, 0);
        wheelInstance.transform.rotation = Quaternion.Euler(90, 0, 90);
        Vector3 pos = wheelInstance.transform.position;
        wheelInstance.transform.position = setupPos(board, wheelInstance, pos, index);
        pos.x += board.transform.position.x;
        pos.z += board.transform.position.z;
        return wheelInstance;
    }

    private Vector3 setupPos(GameObject board, VehicleComponent game, Vector3 pos, int i) // setta la posizione delle route
    {
        float x = board.transform.localScale.x / 2 + game.transform.localScale.y;
        float z = board.transform.localScale.z / 2 + game.transform.localScale.y;
        if (i == 0)
        {
            pos.x += x;
            pos.z += z;
        }
        else if (i == 1)
        {
            pos.x -= x;
            pos.z += z;
        }
        else if (i == 2)
        {
            pos.x += x;
            pos.z -= z;
        }
        else if (i == 3)
        {
            pos.x -= x;
            pos.z -= z;
        }
        return pos;
    }

    private VehicleComponent copyObject(VehicleComponent component) // copia l'oggetto dato
    {
        return Instantiate(component) as VehicleComponent;
    }

    private GameObject createObject(PrimitiveType type, string name, int x, int y, int z)
    {
        GameObject game = GameObject.CreatePrimitive(type);
        game.name = name;
        game.transform.localScale = new Vector3(x, y, z);
        return game;
    }

    private void setStats(Dictionary<string,VehicleComponent> set) //aggiorna le statistiche
    {
        if(set.Count==4)
        {
            attack = set["cannon"].values[0];
            statString(attack, "Attack");
            defense = set["armor"].values[0];
            statString(defense, "Defense");
            speed = set["engine"].values[0];
            statString(speed, "Speed");
            acceleration = set["engine"].values[1];
            statString(acceleration, "Acceleration");
            maneuverability = set["armor"].values[1] + set["wheel"].values[0];
            statString(maneuverability, "Maneuverability");
        }
    }



    private void statString(float stat, string name) //scrive la statistica data sul sul tabellone
    {
        Text[] comps = statsView.GetComponentsInChildren<Text>();
        foreach (Text comp in comps)
            if (comp.name.Equals(name))
                comp.GetComponentsInChildren<Text>()[1].text = stat.ToString();
    }

    public NetworkVehicle createNetworkInstance()
    {
        return net;
    }
}