using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateMatch : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        InputField textView = transform.parent.GetChild(4).gameObject.GetComponent<InputField>();
        string matchName = textView.text;
        startMatch(matchName);
    }

    private void startMatch(string matchName)
    {
        MatchMaker matchMaker = GameObject.Find("MatchMaker").GetComponent<MatchMaker>();
        matchMaker.CreateInternetMatch(matchName);
    }
}
