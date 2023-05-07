using System;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

// Camera Smooth Spring System
public class SmoothSpringCamera : MonoBehaviour
{
    
    [Header("Camera Basic Settings")]
    [SerializeField] private GameObject _target;
    [SerializeField] private bool _followInY = true;
    [SerializeField] private bool _followInX = true;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private float _cameraSpeed = 0.5f;
    
    [Header("Increment Appliance")]
    [SerializeField] private bool _smoothFollowInY = true;
    [SerializeField] private bool _smoothFollowInX = true;
    [Tooltip("if the Smootheningg is below the limits, it will use the limits, too small increments can lead to weird images")]
    [SerializeField] private Vector2 _minimumIncrement = new Vector2(0.0025f, 0.005f);
    
    [FormerlySerializedAs("_useSmootheningArea")]
    [Header("SmootheninggArea")]
    [Tooltip("When the target is out of this area, the camera will simply follow it at the same speed that it moves + a little increment without smoothening")]
    [SerializeField] private bool _useSmootheninggArea = true;
    [SerializeField] private Vector2 _SmootheningAreaOffset;
    [FormerlySerializedAs("_SmootheningAreaHalfSize")] [SerializeField] private Vector2 _SmootheningAreaSize;
    private Vector2 SmootheningAreaPosition => new Vector2(transform.position.x + _SmootheningAreaOffset.x, transform.position.y + _SmootheningAreaOffset.y);
    private bool _isTargetInSmootheningArea = false;
    
    
    private void FixedUpdate()
    {
        // finds player or quits the method
        if (_target is null)
        {
            _target = GameObject.FindWithTag("Player");
            if (_target is null) return;
        }

        // if the target is out of the smoothening area, it wont smooth it, just har follow and just return, in order to prevent it from escaping the area
        _isTargetInSmootheningArea = false;      
        if (_useSmootheninggArea)
        {
            _isTargetInSmootheningArea = IsPointInsideSquare(_target.transform.position, SmootheningAreaPosition, _SmootheningAreaSize);
            if (!_isTargetInSmootheningArea)
            {
                Vector2 distanceToTarget = ((Vector2)_target.transform.position) - SmootheningAreaPosition;
                //newPosition2.x += 
                //return;
            }
        }

        // Otherwise, just applies regular smoothening
        Vector3 targetPosition = _target.transform.position - new Vector3(_offset.x, _offset.y, 0);
        Vector3 newPosition = transform.position;
        Vector3 error = targetPosition - newPosition;
        Vector3 increment = error * _cameraSpeed * Mathf.Min(Time.fixedDeltaTime / _cameraSpeed, 1);
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
    
    bool IsPointInsideSquare(Vector2 point, Vector2 squareCenter, Vector2 squareSize)
    {
        float minX = squareCenter.x - squareSize.x / 2;
        float maxX = squareCenter.x + squareSize.x / 2;
        float minY = squareCenter.y - squareSize.y / 2;
        float maxY = squareCenter.y + squareSize.y / 2;
        return (point.x >= minX && point.x <= maxX && point.y >= minY && point.y <= maxY);
    }

    private void OnDrawGizmos()
    {
        if (!_useSmootheninggArea)
            return;
        
        Gizmos.color = _isTargetInSmootheningArea? Color.green : Color.red;
        Gizmos.DrawWireCube(SmootheningAreaPosition, _SmootheningAreaSize);
    }
    
    
}

