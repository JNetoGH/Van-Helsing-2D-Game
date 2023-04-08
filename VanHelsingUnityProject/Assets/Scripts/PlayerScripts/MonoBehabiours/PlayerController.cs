using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

// todo Dash cool down

public enum Direction
{
    Left,
    Right
}

public class PlayerController : MonoBehaviour
{

    // Observer Pattern
    private List<IPlayerObserver> _playerObservers = new List<IPlayerObserver>();
    
    [SerializeField] private Direction spriteDefaultFacingDirection = Direction.Right;

    private static float InputX => Input.GetAxis("Horizontal");
    
    public static float MaxWalkSpeed => 1.5f;
    private float _runSpeed = 3.0f;
    private float _jumpForce = 4.0f;
    private float _doubleJumpForce = 4.5f;
    
    private const float DashDurationInSeg = 0.1f;
    private const float DashSpeed = 13;
    private float _dashTimer = 0;

    private Vector3 _facingRightScale;
    private Vector3 _facingLeftScale;
    
    private Rigidbody2D _rb;
    private GroundSensorController _groundSensor;

    // Walk / Run / Direction Related States
    public static Direction FacingDirection { get; private set; }
    public static bool IsMovingBackwards { get; private set; } = false;
    public static bool IsLockingToWalkOnly { get; private set; } = false;
    public static bool IsDashing { get; private set; } = false;
    
    // Jump Related States
    public static bool IsGrounded { get; private set; } = false;
    public static bool HasJumpedThisFrame { get; private set; } = false;
    public static bool HasDoubleJumpedThisFrame { get; private set; } = false;
    public static bool IsJumping { get; private set; } = false;
    public static bool IsDoubleJumping { get; private set; } = false;
    public static bool IsShooting { get; private set; } = false;
    
    // To be implemented
    // public static bool IsCombatIdle { get; private set; } = false;
    // public static bool IsDead { get; private set; } = false;
 
    
    // ===============================================================================================================
    // ===============================================================================================================
    

    private void Start()
    {
        
        // adding observers
        // Ideally is the last one to be notified
        _playerObservers.Add(GetComponent<PlayerAnimationObserver>());
        
        // _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensorController>();
        FacingDirection = spriteDefaultFacingDirection;
        
        // scale for sprite sync
        Vector3 currentScale = transform.localScale;
        _facingRightScale = currentScale;
        _facingLeftScale = currentScale;
        if (spriteDefaultFacingDirection == Direction.Right)
            _facingLeftScale.x *= -1;
        else if (spriteDefaultFacingDirection == Direction.Left)
            _facingRightScale.x *= -1;
    }
    
    private void Update()
    {
        UpdateIsDashing();
        if (IsDashing) 
            return;
        
        // Updates isShooting
        IsShooting = Input.GetButton("Shoot");
        
        // Updates isMovingBackwards
        if (FacingDirection == Direction.Left && InputX > 0)
            IsMovingBackwards = true;
        else if (FacingDirection == Direction.Right && InputX < 0)
            IsMovingBackwards = true;
        else
            IsMovingBackwards = false;
        
        UpdateIsGrounded();
        UpdateFacingDir();
        UpdateSpriteInXAxisScale();
        UpdateHasJumpedThisFrame();
        UpdateHasDoubleJumpedThisFrame();
        IsLockingToWalkOnly = Input.GetButton("Lock to Walk Only");
        
        // Ideally is the last one to be updated
        foreach (IPlayerObserver observer in _playerObservers)
        {
            observer.OnNotify(this);
        }
        
    }

    private void FixedUpdate()
    {
        if (IsDashing) Dash();
        else Move(InputX);
    }
    
    
    // ===============================================================================================================
    // ===============================================================================================================
    
    
    private void UpdateIsDashing()
    {
        // cant dash again while dashing
        if (!IsDashing)
            IsDashing = Input.GetButtonDown("Dash");

        if (IsDashing)
        {
            // cant shoot while dashing
            IsShooting = false;
            _dashTimer += Time.deltaTime;
            if (_dashTimer >= DashDurationInSeg)
            {
                IsDashing = false;
                _dashTimer = 0;
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }
        }
    }
    
    private void UpdateFacingDir()
    {
        Vector3 mouseInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mouseInWorldPos.x > this.transform.position.x)
            FacingDirection = Direction.Right;
        else if (mouseInWorldPos.x < this.transform.position.x)
            FacingDirection = Direction.Left;
    }
    
    private void UpdateSpriteInXAxisScale()
    {
        if (FacingDirection == Direction.Left)
            transform.localScale = _facingLeftScale;
        else if (FacingDirection == Direction.Right)
            transform.localScale = _facingRightScale;
    }

    private void UpdateIsGrounded()
    {
        // Check if character just landed on the ground
        if (!IsGrounded && _groundSensor.State())
        {
            IsGrounded = true;
            IsJumping = false;
            IsDoubleJumping = false;
        }
        // Check if character just started falling
        if (IsGrounded && !_groundSensor.State())
        {
            IsGrounded = false;
            IsJumping = true;
        }
        //_animator.SetBool("Grounded", IsGrounded);
    }

    private void UpdateHasJumpedThisFrame()
    {
        HasJumpedThisFrame = Input.GetButtonDown("Jump") && IsGrounded;
        if (!HasJumpedThisFrame)
            return;
        IsGrounded = false;
        IsJumping = true;
        //_animator.SetTrigger("Jump");
        //_animator.SetBool("Grounded", IsGrounded);
        _groundSensor.Disable(0.2f);
        Jump(_jumpForce);
    }

    private void UpdateHasDoubleJumpedThisFrame()
    {
        HasDoubleJumpedThisFrame = Input.GetButtonDown("Jump") && IsJumping && !IsDoubleJumping && !HasJumpedThisFrame;
        if (!HasDoubleJumpedThisFrame)
            return;
        IsDoubleJumping = true;
        //_animator.SetTrigger("DoubleJump");
        Jump(_doubleJumpForce);
    }
    
    
    // ===============================================================================================================
    // ===============================================================================================================
    
    
    private void Dash()
    {
        // In case of not moving dash towards where he is facing
        if (InputX != 0) _rb.velocity = new Vector2(InputX > 0 ? DashSpeed : -DashSpeed, _rb.velocity.y);
        else _rb.velocity = new Vector2(FacingDirection == Direction.Right ? DashSpeed : -DashSpeed, _rb.velocity.y);
    }

    private void Move(float inputX)
    {
        if (IsLockingToWalkOnly) _rb.velocity = new Vector2(inputX * MaxWalkSpeed, _rb.velocity.y);
        else _rb.velocity = new Vector2(inputX * _runSpeed, _rb.velocity.y);
    }

    private void Jump(float force) => _rb.velocity = new Vector2(_rb.velocity.x, force);
    
}
