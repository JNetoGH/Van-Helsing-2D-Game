using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Camera Smooth Spring System
public class CameraController : MonoBehaviour
{
    
    private GameObject _player;
    [SerializeField] private bool followInY = true;
    [SerializeField] private bool followInX = true;
    [SerializeField] private Vector2 offset;
    [SerializeField] private float speed; // Cant be bigger than 1
    
    void Start() => _player = GameObject.FindWithTag("Player");
    
    private void FixedUpdate()
    {
        Vector3 targetPosition = _player.transform.position - new Vector3(offset.x, offset.y, 0); 
        
        Vector3 newPosition = transform.position;
        Vector3 error = targetPosition - newPosition;
        newPosition += error * speed * Mathf.Min(Time.fixedDeltaTime / speed, 1);
        
        if (!followInY)
            newPosition.y = transform.position.y;
        if (!followInX)
            newPosition.x = transform.position.x;
        
        // keep z the same in order to avoid camera rendering bad distances
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
