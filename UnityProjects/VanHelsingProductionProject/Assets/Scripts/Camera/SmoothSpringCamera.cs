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
    
    [Header("Smoothing Area")]
    [SerializeField] private bool _useSmoothingArea = true;
    [SerializeField] private Vector2 _smoothingAreaOffset;
    [SerializeField] private Vector2 _smoothingAreaSize;
    [Tooltip("The speed used by the camera in order to try to catch an object that has escaped the Smoothing Area")]
    [SerializeField] private float _cameraCatchingSpeed = 50;
    
    // Smoothing area internal variables
    private bool _isTargetInSmoothingArea = false;
    private Vector2 SmoothingAreaPosition => new Vector2(
        transform.position.x + _smoothingAreaOffset.x, 
        transform.position.y + _smoothingAreaOffset.y
    );
    
    [Header("Increment Appliance")]
    [SerializeField] private bool _smoothFollowInY = true;
    [SerializeField] private bool _smoothFollowInX = true;
    [Tooltip("if the Smoothing is below the limits, it will use the limits, too small increments can lead to weird images")]
    [SerializeField] private Vector2 _minimumIncrement = new Vector2(0.0025f, 0.005f);
    
    private void FixedUpdate()
    {

        if (_target is null)
        {
            Debug.LogWarning("SmoothSpringCamera target is null");
            return;
        }
        
        _isTargetInSmoothingArea = false;      
        if (_useSmoothingArea)
            _isTargetInSmoothingArea = IsPointInsideSquare(_target.position, SmoothingAreaPosition, _smoothingAreaSize);
        
        Vector3 targetPosition = _target.position - new Vector3(_offsetFromTarget.x, _offsetFromTarget.y, 0);
        Vector3 error = targetPosition - transform.position;
        Vector3 newPosition = Vector3.zero;
        
        // if the target is out of the smoothening area, it wont smooth it in order to prevent it from escaping the area
        if (_isTargetInSmoothingArea || !_useSmoothingArea)
            newPosition = Vector3.Lerp(transform.position, targetPosition, _cameraSpeed * Time.deltaTime);
        else
            newPosition = transform.position + error.normalized * _cameraCatchingSpeed * Time.deltaTime;
        
        // Increments Limits Treatment, too small increments can lead to weird images
        Vector3 increment = newPosition - transform.position;
        if (Mathf.Abs(increment.x) < _minimumIncrement.x) increment.x = 0;
        if (Mathf.Abs(increment.y) < _minimumIncrement.y) increment.y = 0;
        newPosition = transform.position;
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
        if (!_useSmoothingArea)
            return;
       
        Color color = _isTargetInSmoothingArea? Color.green : Color.red;
        Gizmos.color = color;
        Gizmos.DrawWireCube(SmoothingAreaPosition, _smoothingAreaSize);
       
        // Draw the gizmos text
        GUIStyle style = new GUIStyle();
        Vector3 textPosition = SmoothingAreaPosition;
        textPosition.z = transform.position.z;
        textPosition.y += _smoothingAreaSize.y/2 + _smoothingAreaOffset.y + 0.25f;
        style.normal.textColor = color;
        style.alignment = TextAnchor.MiddleCenter;
        Handles.Label(textPosition, "Camera SmoothingArea", style);
    }
    
}

