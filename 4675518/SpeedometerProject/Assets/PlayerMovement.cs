







using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using TMPro;

public class Movement : MonoBehaviour
{
    private Vector3 moveInput;
    private Vector2 mouseInput;
    public float speed;
    public float turnRate = 1;
    public Rigidbody rb;

    public TextMeshProUGUI speedText;
    public float minSpeedArrowAngle;
    public float maxSpeedArrowAngle;
    public float maxSpeed = 0.0f;
    public RectTransform arrow;
    public float playerSpeed;
    
    void Start()
    {
        speed = speed * 22.321f;
    }
    void Update()
    {
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        movePlayer();
        turnPlayer();
        Speedometer();
    }

    private void movePlayer()
    {
        Vector3 moveVector = transform.TransformDirection(moveInput) * speed * Time.fixedDeltaTime;
        rb.linearVelocity = new Vector3(moveVector.x, rb.linearVelocity.y, moveVector.z);
    }
    
    private void turnPlayer()
    {
        transform.Rotate(0f, mouseInput.x * turnRate, 0f);
    }

    private void Speedometer()
    {
        playerSpeed = rb.linearVelocity.magnitude;
        speedText.text = ((int)(playerSpeed * 2.23694)) + " MPH";

        arrow.localEulerAngles =
            new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, playerSpeed / maxSpeed));
    }
}