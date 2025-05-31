using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLifetimeController : MonoBehaviour
{
    ParticleSystem ps;
    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (ps.IsAlive(true) == false) // if all particles are gone and the system wont make more then delete itself
        {
            Destroy(gameObject);
        }
    }
}
