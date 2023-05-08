using UnityEditor;
using UnityEngine;


// Camera Smooth Spring System: by JNeto
public class SmoothSpringCamera : MonoBehaviour
{
    
    [Header("Camera Basic Settings")]
    [SerializeField] private Transform _target;
    [SerializeField] private bool _followInY = true;
    [SerializeField] private bool _followInX = true;
    [SerializeField] private Vector2 _offsetFromTarget;
    [SerializeField] private float _cameraSpeed = 0.5f;
    
    [Header("Smoothening Area")]
    [SerializeField] private bool _useSmootheningArea = true;
    [SerializeField] private Vector2 _smootheningAreaOffset;
    [SerializeField] private Vector2 _smootheningAreaSize;
    [Tooltip("The speed used by the camera in order to try to catch an object that has escaped the Smoothening Area")]
    [SerializeField] private float _cameraCatchingSpeed = 50;
    
    // Smoothening area internal variables
    private bool _isTargetInSmootheningArea = false;
    private Vector2 SmootheningAreaPosition => new Vector2(
        transform.position.x + _smootheningAreaOffset.x, 
        transform.position.y + _smootheningAreaOffset.y
    );
    
    [Header("Increment Appliance")]
    [SerializeField] private bool _smoothFollowInY = true;
    [SerializeField] private bool _smoothFollowInX = true;
    [Tooltip("if the Smoothening is below the limits, it will use the limits, too small increments can lead to weird images")]
    [SerializeField] private Vector2 _minimumIncrement = new Vector2(0.0025f, 0.005f);
    
    private void FixedUpdate()
    {

        if (_target is null)
        {
            Debug.LogWarning("SmoothSpringCamera target is null");
            return;
        }
        
        _isTargetInSmootheningArea = false;      
        if (_useSmootheningArea)
            _isTargetInSmootheningArea = IsPointInsideSquare(_target.position, SmootheningAreaPosition, _smootheningAreaSize);
        
        Vector3 targetPosition = _target.position - new Vector3(_offsetFromTarget.x, _offsetFromTarget.y, 0);
        Vector3 newPosition = transform.position;
        Vector3 error = targetPosition - newPosition;
        Vector3 increment;
        
        // if the target is out of the smoothening area, it wont smooth it in order to prevent it from escaping the area
        if (_isTargetInSmootheningArea || !_useSmootheningArea)
            increment = error * _cameraSpeed * Mathf.Min(Time.fixedDeltaTime / _cameraSpeed, 1);
        else
            increment = error.normalized * _cameraCatchingSpeed * Time.fixedDeltaTime;
        
        // Increments Limits Treatment, too small increments can lead to weird images
        if (Mathf.Abs(increment.x) < _minimumIncrement.x) increment.x = 0;
        if (Mathf.Abs(increment.y) < _minimumIncrement.y) increment.y = 0;
        
        // Applies the increment to the new postion
        newPosition += increment;
        
        // Increment Post-Treatment: uses the user configuration
        if (!_smoothFollowInX) newPosition.x = targetPosition.x;
        if (!_smoothFollowInY) newPosition.y = targetPosition.y;
        if (!_followInY) newPosition.y = transform.position.y;
        if (!_followInX) newPosition.x = transform.position.x;
        
        // Keeps z the same in order to avoid camera rendering bad distances
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        
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
        if (!_useSmootheningArea)
            return;
       
        Color color = _isTargetInSmootheningArea? Color.green : Color.red;
        Gizmos.color = color;
        Gizmos.DrawWireCube(SmootheningAreaPosition, _smootheningAreaSize);
       
        // Draw the gizmos text
        GUIStyle style = new GUIStyle();
        Vector3 textPosition = SmootheningAreaPosition;
        textPosition.z = transform.position.z;
        textPosition.y += _smootheningAreaSize.y/2 + _smootheningAreaOffset.y + 0.25f;
        style.normal.textColor = color;
        style.alignment = TextAnchor.MiddleCenter;
        Handles.Label(textPosition, "Camera SmootheningArea", style);      
        
    }
    
}

