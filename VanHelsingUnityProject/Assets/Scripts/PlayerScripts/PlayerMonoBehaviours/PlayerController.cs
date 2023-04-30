using System.Collections.Generic;
using PlayerScripts.Enums;
using PlayerScripts.Interfaces;
using PlayerScripts.PlayerObservers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private readonly List<IPlayerObserver> _playerObservers = new List<IPlayerObserver>();
    
    
    #region Components
    private Rigidbody2D _rb;
    private GroundSensorController _groundSensor;
    #endregion
    
    
    #region Fields (Move/Jump)
    [SerializeField] private float maxWalkSpeed = 1.5f;
    [SerializeField] private float runSpeed = 3.0f;
    [SerializeField] private float jumpForce = 5.5f;
    #endregion
    
    
    #region Fields (Dash)
    private const float DashDurationInSeg = 0.1f;
    private const float DashSpeed = 13;
    private float _dashTimer = 0;
    #endregion
    
    public bool IsJumping => _rb.velocity.y > 0;
    public bool IsFalling => _rb.velocity.y < 0;
    public bool IsGrounded => _groundSensor.State() && _rb.velocity.y == 0;
    public bool HasJumpedThisFrame =>  Input.GetButtonDown("Jump") && IsGrounded;
    public float InputX => Input.GetAxis("Horizontal");
    public FacingDirection CurrentFacingDirection { get; private set; }
    public bool IsMoving => Mathf.Abs(InputX) > 0f;
    public bool IsMovingBackwards { get; private set; } = false;
    public bool IsLockingToWalkOnly { get; private set; } = false;
    public bool IsDashing { get; private set; } = false;
    public bool IsShooting { get; private set; } = false;
    public bool HasShotThisFrame { get; private set; } = false;

    private void Start()
    {
        // adding observers
        _playerObservers.Add(GetComponent<PlayerSpriteFlipperObserver>());
        _playerObservers.Add(GetComponent<PlayerArmsHandlerObserver>());
        _playerObservers.Add(GetComponent<PlayerAnimationObserver>()); // Ideally is the last one to be notified
        
        // Notifying all Observers that the PlayerController.cs is starting
        foreach (IPlayerObserver observer in _playerObservers)
            observer.OnNotifyStart(this);
        
        _rb = GetComponent<Rigidbody2D>();
        _groundSensor = GetComponentInChildren<GroundSensorController>();
        
        
    }
    
    private void Update()
    {
        // The Player cant do anything else while dashing
        UpdateIsDashing();
        if (IsDashing) 
            return;
        
        if (HasJumpedThisFrame)
            Jump(jumpForce);
        
        // Updates Shooting States (the player cant dash while shooting)
        IsShooting = Input.GetButton("Shoot") || Input.GetButtonDown("Shoot");;
        HasShotThisFrame = Input.GetButtonDown("Shoot");
        
        UpdateCurrentFacingDir();
        UpdateIsMovingBackwards();
        IsLockingToWalkOnly = Input.GetButton("Lock to Walk Only");
        
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

    private void UpdateIsDashing()
    {
        // cant dash again while dashing
        if (!IsDashing)
            IsDashing = Input.GetButtonDown("Dash");

        if (IsDashing)
        {
            // cant shoot while dashing
            HasShotThisFrame = false;
            IsShooting = false;
            
            // releases dash timer
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
    
    private void UpdateIsMovingBackwards()
    {
        if (CurrentFacingDirection == FacingDirection.Left && InputX > 0)
            IsMovingBackwards = true;
        else if (CurrentFacingDirection == FacingDirection.Right && InputX < 0)
            IsMovingBackwards = true;
        else
            IsMovingBackwards = false;
    }

}
