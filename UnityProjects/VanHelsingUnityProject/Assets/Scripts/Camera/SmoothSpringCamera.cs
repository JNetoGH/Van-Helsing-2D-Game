using System;
using Unity.Burst.CompilerServices;
using UnityEngine;

// Camera Smooth Spring System
public class SmoothSpringCamera : MonoBehaviour
{
    
    [Header("Camera Basic Settings")]
    [SerializeField] private GameObject _target;
    [SerializeField] private bool _followInY = true;
    [SerializeField] private bool _followInX = true;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private float _speed = 0.5f;
    
    [Header("Increment Appliance")]
    [SerializeField] private bool _smoothFollowInY = true;
    [SerializeField] private bool _smoothFollowInX = true;
    [SerializeField] private Vector2 _minimumIncrement = new Vector2(0.0025f, 0.005f);

    [Header("Smothering Box")]
    [Tooltip("When the target is out of this box, the camera will simply follow it at the same speed tha it moves")]
    [SerializeField] private bool _useSmotheringBox;
    [SerializeField] private Vector2 _boxOffset;
    [SerializeField] private Vector2 _boxSize;
    private Vector2 SmotheringBoxPosition => new Vector2(transform.position.x + _boxOffset.x, transform.position.y + _boxOffset.y);
    
    private void FixedUpdate()
    {
        // finds player or quits the method
        if (_target is null)
        {
            _target = GameObject.FindWithTag("Player");
            if (_target is null) return;
        }

        if (_useSmotheringBox)
        {
            
        }
        
        
        Vector3 targetPosition = _target.transform.position - new Vector3(_offset.x, _offset.y, 0);
        Vector3 newPosition = transform.position;
        Vector3 error = targetPosition - newPosition;
        
        Vector3 increment = error * _speed * Mathf.Min(Time.fixedDeltaTime / _speed, 1);
        if (Mathf.Abs(increment.x) < _minimumIncrement.x)
            increment.x = 0;
        if (Mathf.Abs(increment.y) < _minimumIncrement.y)
            increment.y = 0;
        
        newPosition += increment;

        if (!_smoothFollowInX)
            newPosition.x = targetPosition.x;
        if (!_smoothFollowInY)
            newPosition.y = targetPosition.y;
        if (!_followInY)
            newPosition.y = transform.position.y;
        if (!_followInX)
            newPosition.x = transform.position.x;
        
        // keep z the same in order to avoid camera rendering bad distances
        newPosition.z = transform.position.z;
        transform.position = newPosition;
        
    }

    private void OnDrawGizmos()
    {
        if (_useSmotheringBox)
            Gizmos.DrawWireCube(SmotheringBoxPosition, _boxSize);
    }
    
}

