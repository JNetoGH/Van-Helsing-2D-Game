using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private GameObject _player;
    [SerializeField] private float limitInYDownWards = 0;
    
    void Start() => _player = GameObject.FindWithTag("Player");
    
    void Update()
    {
        transform.position = transform.position = new Vector3(0, _player.transform.position.y, transform.position.z);
        if (transform.position.y < limitInYDownWards)
            transform.position = new Vector3(transform.position.x, limitInYDownWards, transform.position.z);
    }
    
}
