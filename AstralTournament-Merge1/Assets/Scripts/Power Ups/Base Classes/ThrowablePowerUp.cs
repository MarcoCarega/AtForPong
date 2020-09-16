using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThrowablePowerUp : PowerUp
{
    public abstract void OnThrow(Vector3 force);

    protected override void OnUsePowerUp(NetworkVehicle net,Vector3 force)
    {
        //Vector3 force = CalcThrowVector();
        /*GameObject tmp = new GameObject();
        tmp.transform.position = force;
        tmp.transform.rotation = net.transform.rotation;
        GameObject finalTmp = new GameObject();
        finalTmp.transform.position = tmp.transform.position + force;
        force = finalTmp.transform.position - tmp.transform.position;
        
        Destroy(tmp);
        Destroy(finalTmp);*/
        
        OnThrow(force);
    }

    private Vector3 CalcThrowVector()
    {
        Vector3 center = center = new Vector3(Screen.width / 2, Screen.height / 2, 0); 
        //print("CENTER: " + center);
        Vector3 mouseVector = Input.mousePosition - center;
        //print("MOUSE: " + mouseVector);
        Vector3 throwVector = (Vector3.forward + mouseVector.normalized).normalized * mouseVector.magnitude * 10;

        //print("THROW: " + throwVector);
        return throwVector;
        /*Vector3 center= new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 mouseVector = (Input.mousePosition - center).normalized;
        Vector3 throwVector = (Vector3.forward + mouseVector.normalized).normalized * 500;
        return throwVector;*/
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
