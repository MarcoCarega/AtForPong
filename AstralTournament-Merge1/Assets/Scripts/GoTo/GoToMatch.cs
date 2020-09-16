using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//classe che abilita il tasto 
public class GoToMatch : MonoBehaviour
{
    public Button next;

    private Global global;
    private Vector3 nextPos;
    private bool done;

    // Start is called before the first frame update
    void Start()
    {
        done = false;
        //disattivo bottone mettendo fuori da UI
        nextPos = next.transform.position;
        next.transform.position = new Vector3(nextPos.x + 50, nextPos.y, nextPos.z);
        global = Global.Instance;
        next.onClick.AddListener(onClick);
    }

    private void onClick() //quando il pulsante viene cliccato, si salva l'arena e si passa alla scena del matchmaking
    {
        /*string arena = global.arena;
        arena.transform.SetParent(null);
        DontDestroyOnLoad(arena);*/
        SceneManager.LoadScene("MatchmakeScene");
        //SceneManager.LoadScene("LobbyScene");
    }

    // Update is called once per frame
    void Update() //se è stata selezionata un'arena, allora il bottone viene abilitato
    {
        if(global.hasArena() && !done)
        {
            next.transform.position = nextPos;
            done = true;
        }
    }
}
