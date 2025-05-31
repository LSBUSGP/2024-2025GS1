using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedometerController : MonoBehaviour
{
    [SerializeField]
    GameObject speedometer;
    [SerializeField]
    GameObject dial;
    [SerializeField]
    GameObject player;
    [SerializeField]
    float minAngle;
    [SerializeField]
    float maxAngle;
    [SerializeField]
    float minSpeed;
    [SerializeField]
    float maxSpeed;

    Rigidbody playerRB;
    TMP_Text spedometerText;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = player.GetComponent<Rigidbody>();
        spedometerText = speedometer.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        spedometerText.text = playerRB.velocity.magnitude.ToString("0.00") + " m/s";
        Console.WriteLine(playerRB.velocity.magnitude);

        dial.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(minAngle, maxAngle, Math.Clamp(playerRB.velocity.magnitude / Math.Abs(minSpeed - maxSpeed), 0f, 1f)));
        // lerp between minAngle and maxAngle by the players speed divided by the difference in the min and max speed of the spedometer (to account for negative min speeds, etc)
    }
}
