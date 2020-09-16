using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject focus;
    public float distance = 5f;
    public float height = 2f;
    public float dampening = .8f;
    [HideInInspector]
    public int cameraMode = 0; //0 outside, 1 inside
    public float h2 = 0f;
    public float d2 = 0f;
    public float l = 0f;


    /*private void Start()
    {
        focus = Global.Instance.player.gameObject;
        Debug.Log(focus);
    }*/

    private void Start()
    {
        focus = GameObject.Find("LocalVehicle");
    }

    // Update is called once per frame
    void Update()
    {
        if (focus == null)
        {
           
            try
            {
                focus = GameObject.Find("LocalVehicle");
                //focus = Global.Instance.player.gameObject;
            }
            catch
            {

            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            cameraMode = (cameraMode + 1) % 2;
        }
        if (focus == null) return;
        if (cameraMode == 0)
        {
            transform.position = Vector3.Lerp(transform.position, focus.transform.position + focus.transform.TransformDirection(new Vector3(0f, height, -distance)), dampening * Time.deltaTime);
            transform.LookAt(focus.transform);
            Camera.main.fieldOfView = 60f;
            Camera.main.nearClipPlane = .3f;
        }
        else
        {
            transform.position = focus.transform.position + focus.transform.TransformDirection(new Vector3(l, h2, d2));
            transform.rotation = focus.transform.rotation;
            Camera.main.fieldOfView = 90f;
            Camera.main.nearClipPlane = .1f;
        }
    }
}
