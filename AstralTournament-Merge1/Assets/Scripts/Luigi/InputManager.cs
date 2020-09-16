using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [HideInInspector]
    public float throttle;
    [HideInInspector]
    public float steer;
    [HideInInspector]
    public bool brake;

    // Update is called once per frame
    void FixedUpdate()
    {
        throttle = Input.GetAxis("Vertical");
        steer = Input.GetAxis("Horizontal");

        brake = Input.GetKey(KeyCode.Space);
    }
}
