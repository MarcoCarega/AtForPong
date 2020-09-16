using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayClick : MonoBehaviour // classe che indica i bottoni del menu (quelli verdi)
{
    // Start is called before the first frame update
    public Button button;
    private Global global;

    void Start()
    {
        global = Global.Instance;
        if(button!=null)
            button.onClick.AddListener(onClick);
    }

    public void onClick() //quando il bottone viene cliccato, si passa alla schemata successiva
    {
        global.mode = button.GetComponentInChildren<TMP_Text>().text;
        //DontDestroyOnLoad(mode);
        if(button.gameObject.name.Equals("StoreButton")) SceneManager.LoadScene("StoreScene", LoadSceneMode.Single);
        else SceneManager.LoadScene("SelectCharacter", LoadSceneMode.Single);
    }

    private string getText() 
    {
        return button.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
