using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{

    
    void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<ParticleSystem>().Play();
        GetComponent<SpriteRenderer>().enabled=false;
        

    }
}
