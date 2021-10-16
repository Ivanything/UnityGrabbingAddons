using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Vector3 speed;
    float Yrot;
    public Transform cam;
    public Transform grabPos;
    public Vector2 rotationSens;
    Rigidbody grabObj;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        GetComponent<Rigidbody>().velocity = speed.x * (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) + transform.up * GetComponent<Rigidbody>().velocity.y;
        if (Input.GetKey(KeyCode.R) && grabObj)
        {
            grabObj.transform.RotateAround(transform.up, -Input.GetAxis("Mouse X") * rotationSens.x);
            grabObj.transform.RotateAround(transform.right, Input.GetAxis("Mouse Y") * rotationSens.y);
            grabObj.angularVelocity = Vector3.zero;
            cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cam.GetComponent<Camera>().fieldOfView, 40, Time.deltaTime * 5);
        }
        else
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * speed.y, 0);
            Yrot -= Input.GetAxis("Mouse Y") * speed.z;
            Yrot = Mathf.Clamp(Yrot, -80, 80);
            cam.localRotation = Quaternion.Euler(Yrot, 0, 0);
            cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(cam.GetComponent<Camera>().fieldOfView, 60, Time.deltaTime * 5);
        }
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(cam.position, cam.forward, out hit, 3) && hit.rigidbody && hit.transform.tag != "Ungrabbable")
            grabObj = hit.rigidbody;
        else if (Input.GetMouseButtonUp(0))
            grabObj = null;
        GetComponent<LineRenderer>().enabled = grabObj;
        if (grabObj)//If We Are Grabbing Something
        {
            grabObj.velocity = (grabPos.position - grabObj.transform.position) * 10;
            GetComponent<LineRenderer>().SetPositions(new Vector3[] { grabPos.position, grabObj.transform.position });
        }
    }
}