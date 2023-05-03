using PlayerScripts.Interfaces;
using UnityEngine;


public class PlayerBetterJumpObserver : MonoBehaviour, IPlayerObserver
{
    
    [SerializeField, Range(1, 10)] private float _jumpForce;
    [SerializeField, Range(1, 3)] private float _fallGravityScale;
    [SerializeField, Range(1, 3)] private float _lowJumpGravityScale;
    
    private Rigidbody2D _rigidbody;

    public void OnNotifyStart(PlayerController playerController)
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnNotifyUpdate(PlayerController playerController)
    {
        if (playerController.HasJumpedThisFrame)
        {
            Jump(_jumpForce);
            _rigidbody.gravityScale = 1f;
        }
        else if (playerController.IsFalling)
        {
            // prevents floaty jumps by making the fall heavier
            _rigidbody.gravityScale = _fallGravityScale;
        }
        else if (playerController.IsJumping && !Input.GetButton("Jump"))
        {   
            // variable height jump, only applies more gravity when not pressing the jump button
            _rigidbody.gravityScale = _lowJumpGravityScale;
        }
        if (playerController.IsGrounded)
        {
            _rigidbody.gravityScale = 1f;
        }
    }
    
    private void Jump(float force)
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, force);
    }
    
}
