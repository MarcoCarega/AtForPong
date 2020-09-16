using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject vehicle;
    public float distance;
    private Vector3 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 p = transform.position;
        Vector3 q = vehicle.transform.position;
        Vector3 qp = (q - p).normalized * distance;
        transform.position = q-qp;
        transform.LookAt(vehicle.transform.position);
        lastPos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        if(!lastPos.Equals(mousePos))
            transform.Rotate(0, mousePos.x, 0);
        transform.LookAt(vehicle.transform.position);

    }
}
