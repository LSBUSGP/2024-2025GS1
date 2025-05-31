
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public LayerMask layerMask;

    Vector3 velocity;
    float pitch;

    public float mouseSpeed = 1.5f;
    public float zoomSpeed = 1000;
    public float distance = 10;

    public float maxPitch = 60;
    public float minPitch = -25;

    public float maxZoom = 15;
    public float minZoom = 2;

    public float radius = 0.5f;
    public float smoothTime = 0.1f;

    void Update()
    {
        //finding camera up/down angle
        pitch = Mathf.Clamp(pitch + Input.GetAxis("Mouse Y") * mouseSpeed, -maxPitch, -minPitch);
        //zooming in or out from scrollwheel
        distance = Mathf.Clamp(distance + Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime, minZoom, maxZoom);

        //camera move around the player
        Quaternion rotation = Quaternion.AngleAxis(-pitch, transform.right);
        Vector3 lookDirection = rotation * target.forward;
        Vector3 lookUp = rotation * target.up;

        float d = distance;
        if (Physics.SphereCast(target.position, radius, -lookDirection, out RaycastHit hitInfo, distance, layerMask))
        {
            d = hitInfo.distance * .95f;
        }

        Vector3 cameraTarget = target.position - lookDirection * d;
        transform.position = Vector3.SmoothDamp(transform.position, cameraTarget, ref velocity, smoothTime);
        transform.LookAt(target.position, lookUp);
    }
}