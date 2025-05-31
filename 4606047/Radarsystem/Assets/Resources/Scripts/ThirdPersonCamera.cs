using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public float speed = 90;
    public float distance = 10;
    public float scrollSensitivity = 100;
    public float minimumScroll = 1;
    public float maximumScroll = 20;
    public Transform target;
    float pitch = 0;
    public float maximumPitch = 75;
    public float minimumPitch = -10;
    public float radius = 1;
    public float smoothTime = 1;
    public LayerMask layerMask;
    Vector3 velocity;

    void Update()
    {
        pitch = Mathf.Clamp(pitch + Input.GetAxis("Mouse Y"), minimumPitch, maximumPitch);
        float newDistance = distance + Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSensitivity * distance;
        distance = Mathf.Clamp(newDistance, minimumScroll, maximumScroll);

        Quaternion rotation = Quaternion.AngleAxis(pitch, target.right);
        Vector3 lookDirection = rotation * target.forward;
        Vector3 lookUp = rotation * target.up;
        float d = distance;
        if (Physics.SphereCast(target.position, radius, -lookDirection, out RaycastHit hitInfo, distance, layerMask))
        {
            d = hitInfo.distance;
        }
        Vector3 cameraTarget = target.position - lookDirection * d;
        transform.position = Vector3.SmoothDamp(transform.position, cameraTarget, ref velocity, smoothTime);
        transform.LookAt(target.position, lookUp);
    }
}