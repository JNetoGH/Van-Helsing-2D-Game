using System.Collections.Generic;
using PlayerScripts.Enums;
using PlayerScripts.Interfaces;
using PlayerScripts.PlayerObservers;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    
    private readonly List<IPlayerObserver> _playerObservers = new List<IPlayerObserver>();
    private GroundSensor _groundSensor;
    private Rigidbody2D _rb;

    [Header("Walk and Run")]
    public bool canMove = true;
    [SerializeField] private float _walkMaxSpeed;
    [SerializeField] private float _runSpeed;
    
    [Header("Dash")]
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashDurationInSec;
    [SerializeField] private float _dashCooldownInSec;
    private float _dashDurationTimer = 0;
    private float _dashCooldownTimer = 0;
    public float DashCooldownInSec => _dashCooldownInSec;
    public float DashCooldownTimer => _dashCooldownTimer;
    
    [Header("Dash Cooldown Msg")]
    [SerializeField] private AscendAndDisappearText _ascendAndDisappearText;
    [SerializeField] private Transform _cooldownMsgInstantiationWorldPos;
    
    [Header("Sound Settings")] 
    [SerializeField] private AudioSource _dashAudioSource;
    [SerializeField] private AudioClip _dashAudioClip;
    
    public float InputX => Input.GetAxis("Horizontal");
    public bool IsJumping => _rb.velocity.y > 0;
    public bool IsFalling => _rb.velocity.y < 0;
    public bool IsGrounded => _groundSensor.State;
    public bool IsMoving => Mathf.Abs(InputX) > 0f;
    public bool IsMovingBackwards { get; private set; } = false;
    public bool IsLockingToWalkSpeed { get; private set; } = false;
    public bool IsDashing { get; private set; } = false;
    public FacingDirection CurrentFacingDirection { get; private set; }
    
    private void Start()
    {
        // adding observers
        _playerObservers.Add(GetComponent<PlayerSpriteFlipperObserver>());
        _playerObservers.Add(GetComponent<PlayerBetterJumpObserver>());
        _playerObservers.Add(GetComponent<PlayerArmsHandlerObserver>());
        _playerObservers.Add(GetComponent<PlayerAnimationObserver>()); // Ideally is the last one to be notified
        
        // Notifying all Observers that the PlayerController.cs is starting
        foreach (IPlayerObserver observer in _playerObservers)
            observer.OnNotifyStart(this);
        
        _rb = GetComponent<Rigidbody2D>();
        _groundSensor = GetComponentInChildren<GroundSensor>();
    }
    
    private void Update()
    {
        // The Player cant do anything else while dashing
        _dashCooldownTimer -= Time.deltaTime;
        // needs to be zero in order to sync with the GUI slider
        if (_dashCooldownTimer < 0)
            _dashCooldownTimer = 0;
        
        UpdateIsDashing();
        if (IsDashing) return;
        
        if (!canMove) return;
        
        UpdateCurrentFacingDir();
        UpdateIsMovingBackwards();
        IsLockingToWalkSpeed = Input.GetButton("Lock to Walk Only");
        
        // Notifying all Observers that the PlayerController.cs is updating
        foreach (IPlayerObserver observer in _playerObservers)
            observer.OnNotifyUpdate(this);
    }
    
    private void FixedUpdate()
    {
        if (!canMove) return;
        if (IsDashing) Dash();
        else Move(InputX);
    }
    
    private void Dash()
    {
        // This version of dash ignores gravity
        // In case of not moving dash towards where he is facing
        if (IsMoving) _rb.velocity = new Vector2(InputX > 0 ? _dashSpeed : -_dashSpeed, 0);
        else _rb.velocity = new Vector2(CurrentFacingDirection == FacingDirection.Right ? _dashSpeed : -_dashSpeed, 0);
        if (!_dashAudioSource.isPlaying)
            _dashAudioSource.PlayOneShot(_dashAudioClip);
    }

    private void Move(float inputX)
    {
        if (IsLockingToWalkSpeed) _rb.velocity = new Vector2(inputX * _walkMaxSpeed, _rb.velocity.y);
        else _rb.velocity = new Vector2(inputX * _runSpeed, _rb.velocity.y);
    }
    
    private void UpdateIsDashing()
    {
        bool hasFinishedCooldown = _dashCooldownTimer <= 0;

        // check dashing input, cant dash again while already dashing
        if (!IsDashing)
        {
            // Cooldown Msg that appears when the player tries to dash but the cool down hasn't finished
            if (Input.GetButtonDown("Dash") && !hasFinishedCooldown)
            {
                var msgPos = _cooldownMsgInstantiationWorldPos.transform.position;
                _ascendAndDisappearText.InstantiateMsgWorldPosition(msgPos);
            }
            
            // Updates is Dashing
            IsDashing = Input.GetButtonDown("Dash") && hasFinishedCooldown;
            
            // inits the cooldown timer to the next dash, in case of a valid input to a dash
            if (IsDashing) _dashCooldownTimer = _dashCooldownInSec;
        }
        
        if (IsDashing)
        {
            // updates the dash duration timer
            _dashDurationTimer += Time.deltaTime;
            // in case the duration of the dash has finished
            if (_dashDurationTimer >= _dashDurationInSec)
            {
                IsDashing = false;
                // resets the duration timer
                _dashDurationTimer = 0;
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }
        }
        
    }
    
    private void UpdateCurrentFacingDir()
    {
        Vector3 mouseInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mouseInWorldPos.x > transform.position.x) CurrentFacingDirection = FacingDirection.Right;
        else if (mouseInWorldPos.x < transform.position.x) CurrentFacingDirection = FacingDirection.Left;
    }
    
    private void UpdateIsMovingBackwards()
    {
        if (CurrentFacingDirection == FacingDirection.Left && InputX > 0) IsMovingBackwards = true;
        else if (CurrentFacingDirection == FacingDirection.Right && InputX < 0) IsMovingBackwards = true;
        else IsMovingBackwards = false;
    }

}
