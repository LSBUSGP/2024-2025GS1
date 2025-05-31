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

    public TextMeshProUGUI FPSText;
    public float averageFPS;
    private float contFPS;
    private int count;
    private float timer;
    private float deltaTime = 0f;
    
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
        average();
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

    private void FPS()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        
        averageFPS = Mathf.RoundToInt(1/Time.deltaTime);
        FPSText.text = (averageFPS) + " FPS";
    }
    private void average()
    {
        count++;
        timer += Time.deltaTime;
        contFPS += 1/Time.deltaTime;
        if (timer >= 1)
        {
            averageFPS = contFPS/count;
            count = 0;
            contFPS = 0;
            timer = 0;
            averageFPS = Mathf.RoundToInt(averageFPS);
            FPSText.text = (averageFPS) + " FPS";
        }
    }
}