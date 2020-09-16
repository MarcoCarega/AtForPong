using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//classe che permette il passaggio dal menu alla selezione personaggio
public class GoNext : MonoBehaviour
{
    public Button next;
    private bool done;
    private Global global;
    private Vector3 nextPos;
    public RawImage selectedCharacter;
    public Text story;
    

    // Start is called before the first frame update
    void Start()
    {
        nextPos = next.transform.position;
        next.transform.position = new Vector3(nextPos.x + 50, nextPos.y, nextPos.z);
        next.onClick.AddListener(onClick);
        global = Global.Instance;
    }

    private void onClick() //se premuto, si salva il personaggio in global e si va all schermata del veicolo
    {
        /*Character c = global.character
        c.transform.SetParent(null);
        DontDestroyOnLoad(c);*/
        SceneManager.LoadScene("BuildVehicle");
    }

    // Update is called once per frame
    void Update()
    {
        if(global.hasCharacter()) //se un personaggio è stato scelto, si mostra a schermo la foto del personaggio scelto ( per ora si mostra il colore dato che le immagini non sono ancora pronte) 
        {
            if (!done) //viene abilitato il bottone per andare alla prossima schermata
            { 
                next.transform.position = nextPos;
                done = true;
            }
            Character c = Resources.Load<GameObject>("Prefabs/Characters/"+global.character).GetComponent<Character>();
            selectedCharacter.color = new Color(c.color.r, c.color.g, c.color.b, 1);
            story.text = c.charactName;
            //story.enabled = true;
            //selectedCharacter.enabled = true;
            //selectedCharacter.ac
        }
        
        //print(selectedCharacter.color);
    }
}
