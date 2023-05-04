using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    [SerializeField] private GameObject particles;
    [SerializeField] private Transform particlesInstantiationPoint;

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject obj = GameObject.Instantiate(particles);
        obj.transform.position = particlesInstantiationPoint.position;
        Debug.Log("OnCollisionEnter2D");
        GameObject.Destroy(this.gameObject);
        
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
