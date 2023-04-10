using System.Collections.Generic;
using PlayerScripts.Enums;
using PlayerScripts.Interfaces;
using PlayerScripts.PlayerObservers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private List<IPlayerObserver> _playerObservers = new List<IPlayerObserver>();
    
    
    #region Components
    private Rigidbody2D _rb;
    private GroundSensorController _groundSensor;
    #endregion
    
    
    #region Fields (Move/Jump)
    [HideInInspector] public float maxWalkSpeed = 1.5f;
    [HideInInspector] public float runSpeed = 3.0f;
    [HideInInspector] public float jumpForce = 4.0f;
    [HideInInspector] public float doubleJumpForce = 4.5f;
    #endregion
    
    
    #region Fields (Dash)
    private const float DashDurationInSeg = 0.1f;
    private const float DashSpeed = 13;
    private float _dashTimer = 0;
    #endregion
    

    #region Get-Only Static States for Subsystems
    public static float InputX => Input.GetAxis("Horizontal");
    public static FacingDirection CurrentFacingDirection { get; private set; }
    public static bool IsMoving => Mathf.Abs(InputX) > 0f;
    public static bool IsMovingBackwards { get; private set; } = false;
    public static bool IsLockingToWalkOnly { get; private set; } = false;
    public static bool IsDashing { get; private set; } = false;
    public static bool IsGrounded { get; private set; } = false;
    public static bool HasJumpedThisFrame { get; private set; } = false;
    public static bool HasDoubleJumpedThisFrame { get; private set; } = false;
    public static bool IsJumping { get; private set; } = false;
    public static bool IsDoubleJumping { get; private set; } = false;
    public static bool IsShooting { get; private set; } = false;
    #endregion
    
    
    // ===============================================================================================================
    // ===============================================================================================================
    

    private void Start()
    {
        // adding observers
        _playerObservers.Add(GetComponent<PlayerSpriteFlipperObserver>());
        _playerObservers.Add(GetComponent<PlayerAnimationObserver>()); // Ideally is the last one to be notified
        
        // Notifying all Observers that the PlayerController.cs is starting
        foreach (IPlayerObserver observer in _playerObservers)
            observer.OnNotifyStart(this);
        
        _rb = GetComponent<Rigidbody2D>();
        _groundSensor = GetComponentInChildren<GroundSensorController>();
    }
    
    private void Update()
    {
        UpdateIsDashing();
        if (IsDashing) 
            return;
        
        // Updates isShooting
        IsShooting = Input.GetButton("Shoot");
        
        // Updates isMovingBackwards
        if (CurrentFacingDirection == FacingDirection.Left && InputX > 0)
            IsMovingBackwards = true;
        else if (CurrentFacingDirection == FacingDirection.Right && InputX < 0)
            IsMovingBackwards = true;
        else
            IsMovingBackwards = false;
        
        UpdateIsGrounded();
        UpdateCurrentFacingDir();
        UpdateHasJumpedThisFrame();
        UpdateHasDoubleJumpedThisFrame();
        IsLockingToWalkOnly = Input.GetButton("Lock to Walk Only");
        
        if (HasJumpedThisFrame)
            Jump(jumpForce);

        if (HasDoubleJumpedThisFrame)
            Jump(doubleJumpForce);
        
        // Notifying all Observers that the PlayerController.cs is updating
        foreach (IPlayerObserver observer in _playerObservers)
            observer.OnNotifyUpdate(this);
    }

    private void FixedUpdate()
    {
        if (IsDashing) Dash();
        else Move(InputX);
    }
    
    private void Dash()
    {
        // In case of not moving dash towards where he is facing
        if (IsMoving) _rb.velocity = new Vector2(InputX > 0 ? DashSpeed : -DashSpeed, _rb.velocity.y);
        else _rb.velocity = new Vector2(CurrentFacingDirection == FacingDirection.Right ? DashSpeed : -DashSpeed, _rb.velocity.y);
    }

    private void Move(float inputX)
    {
        if (IsLockingToWalkOnly) _rb.velocity = new Vector2(inputX * maxWalkSpeed, _rb.velocity.y);
        else _rb.velocity = new Vector2(inputX * runSpeed, _rb.velocity.y);
    }

    private void Jump(float force) => _rb.velocity = new Vector2(_rb.velocity.x, force);
    
    
    // ===============================================================================================================
    // ===============================================================================================================
    //                                        UPDATE STATES
    
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
    
    private void UpdateCurrentFacingDir()
    {
        Vector3 mouseInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mouseInWorldPos.x > this.transform.position.x) CurrentFacingDirection = FacingDirection.Right;
        else if (mouseInWorldPos.x < this.transform.position.x) CurrentFacingDirection = FacingDirection.Left;
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
        _groundSensor.Disable(0.2f);
    }

    private void UpdateHasDoubleJumpedThisFrame()
    {
        HasDoubleJumpedThisFrame = Input.GetButtonDown("Jump") && IsJumping && !IsDoubleJumping && !HasJumpedThisFrame;
        if (!HasDoubleJumpedThisFrame)
            return;
        IsDoubleJumping = true;
    }
    
}
