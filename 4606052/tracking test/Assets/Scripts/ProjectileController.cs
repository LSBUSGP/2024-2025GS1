using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] 
    public float speed; // meters per second
    [SerializeField]
    public float lifetime = 1f; // seconds
    [SerializeField]
    public GameObject hitEffect;
    float? delay;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D m_Rigidbody = GetComponent<Rigidbody2D>(); // get own rigidbody
        m_Rigidbody.velocity = transform.up * speed; // set velocity to speed variable
    }
    void FixedUpdate()
    {
        delay = delay ?? Time.time + lifetime; // if no delay exists then start the delay
        if (Time.time >= delay) // if delay has passed then delete self
        {
            Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision) 
    { 
        Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    } 
}
