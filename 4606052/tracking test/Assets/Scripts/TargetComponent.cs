using UnityEngine;

public class TargetComponent : MonoBehaviour // this component can be entirely empty, i have only filled it for demonstration reasons.
{
    Rigidbody2D rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        transform.Rotate(0, 0, Random.Range(0, 360f));
        rigid.AddForce(transform.up * 100); // throw targets in random directions on spawn
    }
    void Update()
    {
        if (Input.GetKeyDown("space")) // press space to bounce targets around
        {
            rigid.AddForce(transform.up * 100);
        }
    }
}