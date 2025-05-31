using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public float speed = 10f;
    public float mouseX = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 15f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 10f;
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 movement = (Vector3.forward * y + Vector3.right * x) * Time.deltaTime * speed;

        transform.Translate(movement, Space.Self);

        float oldMouseX = mouseX;
        mouseX = Input.mousePosition.x;
        float angle = mouseX - oldMouseX;

        transform.Rotate(Vector3.up, angle);
    }
}
