using PlayerScripts.Interfaces;
using UnityEditor;
using UnityEngine;


public class PlayerBetterJumpObserver : MonoBehaviour, IPlayerObserver
{
    
    [Header("Features")]
    [SerializeField] private float _coyoteTimeDurationInSeconds = 0;

    [Header("Anti-Floppy Jump Settings")]
    [SerializeField, Range(1, 10)] private float _jumpForce;
    [SerializeField, Range(1, 3)] private float _fallGravityScale;
    [SerializeField, Range(1, 3)] private float _lowJumpGravityScale;
    
    private bool HasJumpedThisFrame => Input.GetButtonDown("Jump");
    private float _coyoteTimeCountDownTimer = 0;
    private bool _wasGroundedLastFrame = false;
    private Rigidbody2D _rigidbody;
        
    public void OnNotifyStart(PlayerController playerController)
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnNotifyUpdate(PlayerController playerController)
    {
        
        // Updating _coyoteTimeCountDownTimer
        _coyoteTimeCountDownTimer -= Time.deltaTime;
        if (_coyoteTimeCountDownTimer < 0) _coyoteTimeCountDownTimer = 0;
        
        // Checking if coyote time should be released
        bool releaseCoyoteTimer = _wasGroundedLastFrame && playerController.IsFalling;
        if (releaseCoyoteTimer)
            _coyoteTimeCountDownTimer = _coyoteTimeDurationInSeconds;
        _wasGroundedLastFrame = playerController.IsGrounded;
        
        // Jumps if it's grounded or if it's in coyote time
        if (HasJumpedThisFrame)
        {
            if (playerController.IsGrounded || _coyoteTimeCountDownTimer > 0)
            {
                Jump(_jumpForce);
                _rigidbody.gravityScale = 1f;
                // disables the coyoteTime
                _wasGroundedLastFrame = false;
                _coyoteTimeCountDownTimer = 0;
            }
        }
        
        // Variable gravity for anti-floppy jumps
        if (playerController.IsJumping && !Input.GetButton("Jump"))
        {
            // variable height jump, only applies more gravity when not pressing the jump button
            _rigidbody.gravityScale = _lowJumpGravityScale;
        }
        else if (playerController.IsFalling)
        {
            // prevents floaty jumps by making the fall heavier
            _rigidbody.gravityScale = _fallGravityScale;
        }
        
    }
    
    private void Jump(float force)
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, force);
    }
    
    private void OnDrawGizmos()
    {
        // Set the gizmo color
        Color color = _coyoteTimeCountDownTimer > 0 ? Color.green : Color.red;
        
        // Draw a sphere at the GameObject's position
        Gizmos.color = color;
        Vector3 spherePosition = transform.position;
        spherePosition.x -= 0.5f; 
        spherePosition.y += 0.5f; 
        Gizmos.DrawSphere(spherePosition, 0.1f);

        // Draw the gizmo text
        Vector3 textPosition = transform.position;
        textPosition.y += 0.5f; 
        GUIStyle style = new GUIStyle();
        style.normal.textColor = color;
        style.alignment = TextAnchor.MiddleCenter;
        Handles.Label(textPosition, "CoyoteTime", style);
    }
    
}
