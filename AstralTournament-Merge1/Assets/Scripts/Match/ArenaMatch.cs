
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class ArenaMatch : MatchBase
{
    private int matchCount;
    private bool done;
    public int numOfEnemies;
    public bool isDied;
    public PlayUI gameUI;
   

    // Start is called before the first frame update
    void Start()
    {
        CustomLobby lobby = GameObject.Find("LobbyManager").GetComponent<CustomLobby>();
        gameUI= GameObject.Find("PlayUI").GetComponent<PlayUI>();
        matchCount = 0;
        isMatchStarted = false;
        numOfEnemies = lobby.numPlayers-1;
        numOfEnemies = 2;
        StartCoroutine("StartMatch");

    }

    // Update is called once per frame
    void Update()
    { 
        if(numOfEnemies==0)
        {
            gameUI.gameStatus.text = "Victory!";

            StopCoroutine("StartMatch");
            matchCount++;
        }
        else if(isDied)
        {
            //Aggiornare UI con SCONFITTA!
            gameUI.gameStatus.text = "Defeat!";
            
            //Disabilita componenti movimento
            //Attendi fine match

            matchCount++;
        }
        if(matchCount==3)
        {
            //Mostra risultati
        }
    }

    private IEnumerator StartMatch()
    {
        yield return new WaitForSeconds(durationInMinutes * 60);
        print("QUI ESCONO GLI OMUNCOLI");
        gameUI.gameStatus.text = "Survive!";
        yield return new WaitForSeconds(3);
        gameUI.gameStatus.text = "";
    }

}
