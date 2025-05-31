using System;
using UnityEngine;
using UnityEngine.UI;


public class Ping : MonoBehaviour
{
    Sprite red, green;
    Image m_Image;
    public Sprite greenPing;
    public Sprite redPing;

    public bool isRed = false;

    void Start()
    {
        m_Image = GetComponent<Image>();

        red = Resources.Load<Sprite>("RadarPingRed");
        green = Resources.Load<Sprite>("RadarPingGreen");
    }
    public void Position(Vector2 position)
    {
        double root = Math.Sqrt(position.x * position.x + position.y * position.y);
        if (root >= 69.5 && isRed == false)
        {
            m_Image.sprite = redPing;
            isRed = true;
        }
        if (root < 69.5 && isRed == true)
        {
            m_Image.sprite = greenPing;
            isRed = false;
        }
        transform.localPosition = position;
    }
}
