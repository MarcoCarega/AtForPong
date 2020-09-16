using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreviousScene : MonoBehaviour
{
    public string previous;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(onClick);
    }

    public void onClick()
    {
        SceneManager.LoadScene(previous);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
