using System;
using UnityEngine;
using UnityEngine.UI;

public class RadarContact : MonoBehaviour
{
    public Ping ping;

    public Transform Player;
    public Vector3 playerRelative;


    void Update()
    {
        playerRelative = Player.InverseTransformPoint(transform.position);
        playerRelative.y = playerRelative.z;
        playerRelative.z = 0f;
        double root = Math.Sqrt(playerRelative.x * playerRelative.x + playerRelative.y * playerRelative.y);
        if (root >= 70)
        {
            float divide = (float)(root / 70);
            playerRelative.x = playerRelative.x / divide;
            playerRelative.y = playerRelative.y / divide;
        }
        ping.Position(playerRelative);
    }
}