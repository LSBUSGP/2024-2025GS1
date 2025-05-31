using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 1;
    public float turnRate = 90;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float yaw = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, yaw * Time.deltaTime * turnRate);

        transform.Translate((Vector3.right * h + Vector3.forward * v) * Time.deltaTime * speed, Space.Self);

    }
}