using System;
using System.Linq;
using UnityEngine;


public class GroundSensor : MonoBehaviour
{
    [Header("Turn ON/OFF")]
    [SerializeField] private bool _enabled = true;
    
    [Header("Sensor Settings")]
    [SerializeField] private Vector2 _size;
    [SerializeField] private Vector2 _offset;
    
    [Header("Layer Setting (Multiple Selection)")]
    [SerializeField] private LayerMask _layersToConsider;

    // Internal Fields and Properties
    private bool _isGrounded = false;
    private Rigidbody2D _rb;
    private Vector2 Center => (Vector2)transform.position + _offset;

    // Used by other scripts to get sensor state
    public bool State => _isGrounded;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Clears it for checking
        _isGrounded = false;
        
        // Checking if it's enabled
        if (!_enabled) return;
        
        // Creating an axis-aligned bounding box at the center position with the given size
        Collider2D[] hits = Physics2D.OverlapBoxAll(Center, _size, 0f);
        
        // Checking if has collided
        bool hasCollided = hits is not null;
        if (!hasCollided)
            return;
        
        // Checking if the box hit collider that is in this game object and if its in a valid layer
        bool anyValidHit = false;
        foreach (Collider2D hit in hits)
        {
            if (this.transform.root == hit.transform.root)
                continue;
            if (_layersToConsider == (_layersToConsider  | (1 << hit.gameObject.layer)))
                anyValidHit = true;
        }
        if (!anyValidHit)
            return;
        
        // Checking if the object is stopped in Y
        bool isStoppedInY = Mathf.Abs(_rb.velocity.y) < 0.1f;
        if (!isStoppedInY)
            return;
        
        _isGrounded = true;
        Debug.Log($"sensor is colliding");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawCube(Center, _size);
    }

    public void Disable(float duration) => _enabled = false;
}