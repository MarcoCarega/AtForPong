
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantMatch : MatchBase
{
    private int matchCount;
    private bool done;
    public int points;
    public bool isDied;
    public bool isGiantDied;

    // Start is called before the first frame update
    void Start()
    {
        CustomLobby lobby = GameObject.Find("LobbyManager").GetComponent<CustomLobby>();
        matchCount = 0;
        isMatchStarted = false;
        //numOfEnemies=numero giocatori avversari
        StartCoroutine("StartMatch");
    }

    // Update is called once per frame
    void Update()
    {

        if (isDied)
        {
            //Aumenta tempo respawn
            //Disabilita componenti movimento
            //Coroutine per respawn
        }
        else if (isGiantDied)
        {
            //Disabilita componenti movimento
            //Mostra risultati
        }
    }

    private IEnumerator StartMatch()
    {
        yield return new WaitForSeconds(durationInMinutes * 60);
        //QUI ESCONO GLI OMUNCOLI

    }
}
