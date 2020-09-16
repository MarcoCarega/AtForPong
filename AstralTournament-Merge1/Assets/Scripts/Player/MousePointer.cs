using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MousePointer : MonoBehaviour
{
    private RawImage pointer;
    private GameObject spawnBullet;
    private Camera camera;

    private Vector3 center;
    
    // Start is called before the first frame update
    void Start()
    {
        RectTransform canvas = GetComponent<RectTransform>();
        //print("SCREEN: " + Screen.width + " - " + Screen.height);
        center = new Vector3(Screen.width/2,Screen.height/2,0);
        pointer = GameObject.Find("PlayUI/crosshair").GetComponent<RawImage>();
        //spawnBullet = GameObject.Find("Player/SpawnBullet");
        //camera = GameObject.Find("Player/Main Camera").GetComponent<Camera>();
        //Vector3 position = camera.transform.position;
        //position.x = center.x;
        //position.y = center.y;
        //camera.transform.position = position;
        //spawnBullet.transform.position = position;
        pointer.transform.position = Input.mousePosition;
    }

    // Update is called once per frame
    void Update() //prove per vedere se spara
    {
       
        pointer.transform.position = Input.mousePosition;
        if(Input.GetKey(KeyCode.X))
        {
            throwObject();
            Thread.Sleep(500);
        }
    }

    private void throwObject()
    {
        GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bullet.transform.localScale *= 3;
        Rigidbody rigid = bullet.AddComponent<Rigidbody>();
        Vector3 throwVector = CalcThrowVector();
        rigid.AddForce(throwVector);
        print(throwVector);
    }

    private Vector3 CalcThrowVector()
    {
        Vector3 center= center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        print("CENTER: " + center);
        Vector3 mouseVector = Input.mousePosition-center;
        print("MOUSE: " + mouseVector);
        Vector3 throwVector = (Vector3.forward+mouseVector.normalized).normalized*mouseVector.magnitude*10;
      
        print("THROW: "+throwVector);
        return throwVector;
    }
}
