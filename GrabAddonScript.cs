using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float Ysensitivity;
    float Yrot;
    Camera cam;
    public Transform grabPos;
    public Vector2 rotationSens;
    Rigidbody grabObj;
    LineRenderer lr;
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        cam = GetComponent<Camera>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.R) && grabObj)
        {
            grabObj.transform.RotateAround(transform.up, -Input.GetAxis("Mouse X") * rotationSens.x);
            grabObj.transform.RotateAround(transform.right, Input.GetAxis("Mouse Y") * rotationSens.y);
            grabObj.angularVelocity = Vector3.zero;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 40, Time.deltaTime * 5);
        }
        else
        {
            Yrot -= Input.GetAxis("Mouse Y") * Ysensitivity;
            Yrot = Mathf.Clamp(Yrot, -80, 80);
            transform.localRotation = Quaternion.Euler(Yrot, 0, 0);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60, Time.deltaTime * 5);
        }
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(transform.position, transform.forward, out hit, 3) && hit.rigidbody && hit.transform.tag != "Ungrabbable")
            grabObj = hit.rigidbody;
        else if (Input.GetMouseButtonUp(0))
            grabObj = null;
        lr.enabled = grabObj;
        if (grabObj)//If We Are Grabbing Something
        {
            grabObj.velocity = (grabPos.position - grabObj.transform.position) * 10;
            lr.SetPositions(new Vector3[] { grabPos.position, grabObj.transform.position });
        }
    }
}