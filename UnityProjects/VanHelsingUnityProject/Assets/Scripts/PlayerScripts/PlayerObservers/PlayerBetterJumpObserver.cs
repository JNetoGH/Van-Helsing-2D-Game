using PlayerScripts.Interfaces;
using UnityEditor;
using UnityEngine;


public class PlayerBetterJumpObserver : MonoBehaviour, IPlayerObserver
{
    
    [Header("Jump Settings")]
    [SerializeField, Range(1, 10)] private float _jumpForce;
    
    [Header("Anti-Floaty Jump Settings")]
    [SerializeField, Range(1, 3)] private float _fallGravityScale;
    
    [Header("Variable Height Jump Settings")]
    [SerializeField, Range(1, 3)] private float _lowJumpGravityScale;
    
    [Header("Coyote Time Settings")]
    [SerializeField] private bool _useCoyoteTime = true;
    [SerializeField] private float _coyoteTimeDurationInSeconds = 0;
    
    [Header("Jump Buffer Settings")]
    [SerializeField] private bool _useJumpBuffer = true;
    [SerializeField] private Vector2 _jumpBufferLineOffset;
    [SerializeField] private float _jumpBufferLineLength;
    
    private bool HasPlayerPressedJumpedFrame => Input.GetButtonDown("Jump");
    private float _coyoteTimeCountDownTimer = 0;
    private bool _wasGroundedLastFrame = false;
    private Rigidbody2D _rigidbody;
    private Vector2 JumpBufferLineStart => new Vector2(
        transform.position.x + _jumpBufferLineOffset.x, 
        transform.position.y + _jumpBufferLineOffset.y);
    private Vector2 JumpBufferLineEnd => new Vector2(JumpBufferLineStart.x, JumpBufferLineStart.y - _jumpBufferLineLength);
    private bool _hasJumpBuffered = false;

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
        if (releaseCoyoteTimer && _useCoyoteTime)
            _coyoteTimeCountDownTimer = _coyoteTimeDurationInSeconds;
        _wasGroundedLastFrame = playerController.IsGrounded;
        
        // Updates the jump buffering
        if (_useJumpBuffer)
        {
            bool hasBufferCollided = CheckJumpBufferCollision();
            if (!hasBufferCollided)
                _hasJumpBuffered = false;
            else if (HasPlayerPressedJumpedFrame && !_hasJumpBuffered)
                _hasJumpBuffered = true;
        }
        else _hasJumpBuffered = false;
        
        // Jumps if it's grounded or if it's in coyote time when the player gives the input or has a jumper buffered
        if (HasPlayerPressedJumpedFrame || _hasJumpBuffered)
        {
            if (playerController.IsGrounded || _coyoteTimeCountDownTimer > 0)
            {
                Jump(_jumpForce);
                _rigidbody.gravityScale = 1f;
                // disables the coyoteTime
                _wasGroundedLastFrame = false;
                _coyoteTimeCountDownTimer = 0;
                _hasJumpBuffered = false;
            }
        }
        
        // Variable gravity for anti-floaty jumps
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

    private bool CheckJumpBufferCollision()
    {
        // Creating an axis-aligned bounding box at the center position with the given size
        RaycastHit2D[] hits = Physics2D.LinecastAll(JumpBufferLineStart, JumpBufferLineEnd);
        
        // Checking if has collided
        bool hasCollided = hits is not null;
        if (!hasCollided)
            return false;
        
        // Checking if the box hit collider that is in this game object
        bool anyValidHit = false;
        foreach (RaycastHit2D hit in hits)
            if (this.transform.root != hit.transform.root)
                anyValidHit = true;
        
        return anyValidHit;
    }

    private void Jump(float force)
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, force);
    }
    
    private void OnDrawGizmos()
    {
        
        if (_useCoyoteTime)
        {
            // Set the gizmo color for the coyote time
            Color colorCoyoteTime = _coyoteTimeCountDownTimer > 0 ? Color.green : Color.red;
        
            // Draw a sphere at the GameObject's for the coyote time
            Gizmos.color = colorCoyoteTime;
            Vector3 spherePosition = transform.position;
            spherePosition.y += 0.5f; 
            Gizmos.DrawSphere(spherePosition, 0.1f);
        
            // Draw the gizmo text for the coyote time
            Vector3 coyoteTextPosition = transform.position;
            coyoteTextPosition.y += 0.5f; 
            coyoteTextPosition.x += 0.75f; 
            GUIStyle coyoteTextStyle = new GUIStyle();
            coyoteTextStyle.normal.textColor = colorCoyoteTime;
            coyoteTextStyle.alignment = TextAnchor.MiddleCenter;
            Handles.Label(coyoteTextPosition, "CoyoteTime", coyoteTextStyle);
        }
        
        if (_useJumpBuffer)
        {
            // Draws a line for the jump buffer
            Color colorJumpBuffer = _hasJumpBuffered ? Color.green : Color.yellow;
            Gizmos.color = colorJumpBuffer;
            Gizmos.DrawLine(JumpBufferLineStart, JumpBufferLineEnd);
        
            // Draw the gizmo text for the coyote time
            Vector3 jumpBufferTxtPosition = JumpBufferLineEnd;
            jumpBufferTxtPosition.x += 0.75f; 
            GUIStyle styleJumpBufferText = new GUIStyle();
            styleJumpBufferText.normal.textColor = colorJumpBuffer;
            styleJumpBufferText.alignment = TextAnchor.MiddleCenter;
            Handles.Label(jumpBufferTxtPosition, "Jump Buffer", styleJumpBufferText);
        }        
        
    }
    
}
