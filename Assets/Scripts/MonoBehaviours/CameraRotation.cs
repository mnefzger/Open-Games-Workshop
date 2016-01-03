﻿using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {

    public int camDistance = 185;
    GameObject planet;
    Camera cam;
    float rotationXAxis_local = 0.0f;
    float rotationYAxis_local = 0.0f;
    float fov;
    public float fovSpeed = 8f;
    public float camSpeed = 5f;
    public float SLERPTIME = 1f;

    void Start () {
        planet = GameObject.Find("Planet");
        cam = Camera.main;
        fov = cam.fieldOfView;
        Vector3 localAngles = cam.transform.localEulerAngles;
        rotationXAxis_local = localAngles.x;
        rotationYAxis_local = localAngles.y;

    }

    void LateUpdate()
    {

        if (Input.GetKey(KeyCode.A))
        {
            Vector3 verticalaxis = transform.TransformDirection(Vector3.down);
            transform.RotateAround(planet.transform.position, verticalaxis, -camSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 verticalaxis = transform.TransformDirection(Vector3.up);
            transform.RotateAround(planet.transform.position, verticalaxis, -camSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 horizontalaxis = transform.TransformDirection(Vector3.left);
            transform.RotateAround(planet.transform.position, horizontalaxis, -camSpeed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 horizontalaxis = transform.TransformDirection(Vector3.right);
            transform.RotateAround(planet.transform.position, horizontalaxis, -camSpeed * Time.deltaTime);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKey(KeyCode.X)) // forward
        {
            if (fov >= 10) fov -= 5;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKey(KeyCode.Y)) // back
        {
            if (fov <= 100) fov += 5;
        }

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, Time.deltaTime * fovSpeed);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FocusOnSpaceship());
        }
    }

    IEnumerator FocusOnSpaceship()
    {

        //Quaternion cameraRotation = Quaternion.Euler(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y, 0);
        //Vector3 cameraPosition = cameraRotation * new Vector3(0, 0, -camDistance) + planet.transform.position;

        float startTime = Time.time;
        fov = 35f;

        Vector3 cameraPosition = GameValues.ShipPos.normalized * camDistance;

        Vector3 fromPos = cam.transform.position;
        // Vector3 fromRot = cam.transform.forward;
        Vector3 fromRot = -transform.position.normalized;

        while (Time.time < startTime + SLERPTIME)
        {
            cam.transform.forward = Vector3.Slerp(fromRot, -GameValues.ShipPos.normalized, (Time.time - startTime) / SLERPTIME);
            cam.transform.position = Vector3.Slerp(fromPos, cameraPosition, (Time.time - startTime) / SLERPTIME);
            yield return null;
        }       

    }


    public int getCamDistance()
    {
        return camDistance;
    }
}
