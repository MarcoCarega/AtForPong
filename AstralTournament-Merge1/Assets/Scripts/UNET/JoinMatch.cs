using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinMatch : MonoBehaviour
{
    public InputField textbox;

    private Global global;
    // Start is called before the first frame update
    void Start()
    {
        global = Global.Instance;
        Button button = GetComponent<Button>();
        button.onClick.AddListener(onClick);
    }

    private void onClick()
    {
        InputField textView = transform.parent.GetChild(3).gameObject.GetComponent<InputField>();
        String matchName = textView.text;
        findMatch(matchName);
    }

    private void findMatch(string matchName)
    {
        MatchMaker matchMaker = GameObject.Find("MatchMaker").GetComponent<MatchMaker>();
        matchMaker.FindInternetMatch(matchName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
