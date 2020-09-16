using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaSelected : MonoBehaviour
{
    public Button button;
    public Arena arena;
    private Global global;
    // Start is called before the first frame update
    void Start()
    {
        global = Global.Instance;
        button.onClick.AddListener(onClick);
    }

    private void onClick()
    {
        global.arena = arena.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
