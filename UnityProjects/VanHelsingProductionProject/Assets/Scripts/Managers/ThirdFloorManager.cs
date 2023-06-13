using System;
using Cinemachine;
using UnityEngine;

public class ThirdFloorManager : MonoBehaviour, IFloorManager
{
    
    // Set by the scene switcher
    public bool IsFloorRunning { get; set; }
    
    // Set by the Level Ignorer Script
    public bool IgnoreLevel { get; set; }
    
    [Header("Camera Settings")]
    [SerializeField] private Movement _cameraMovement;
    [SerializeField] private GameObject _camera;
    
    [Header("Lightning Settings")]
    [SerializeField] private GameObject _lightingPrefab;

    [Header("PLayer Respawn Settings")]
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _playerRespawnPosition;

    // Control Fields
    private GameObject _currentLightingObj;
    private bool _hasInitPhaseForTheFirstTime = false;
    private bool _hasLightiningBeenCreated = false;
    
    // Ignoring Floor Fields
    private float _defaultOrthoSize;
    private Vector3 _cameraInitialPosition;


    private void Start()
    {
        _defaultOrthoSize = _camera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize;
        _cameraInitialPosition = _camera.transform.position;
    }

    public void InitPhase()
    {
        _cameraMovement.enabled = true;
        _cameraMovement.TeleportToPoint(Movement.TargetPoint.PointA);
        _cameraMovement.ResetWaitingTimer();
        PlayerDeathManager.currentFloorManager = this;
    }
    
    // Comes from the Interface, called by LightningController
    public void OnPlayerDead()
    {
        Debug.Log("Player has died in third floor");
        InitPhase();
        
        // Teleports player and kills its velocity
        _player.transform.position = _playerRespawnPosition.position;
        _player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        _player.GetComponent<Animator>().SetBool("IsGrounded", true);
        
        // Also brute forces the animation to default idle
        Animator playerAnimator = _player.GetComponent<Animator>();
        playerAnimator.Play("Idle");
    }
    
    // Update is called once per frame
    void Update()
    {
        
        // Ignoring Floor Implementation
        _cameraMovement.enabled = !IgnoreLevel;
        _camera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = IgnoreLevel ? 8 : _defaultOrthoSize;
        if (IgnoreLevel)
        {
            _camera.transform.position = new Vector3(
                _cameraInitialPosition.x, 
                _cameraInitialPosition.y + 6,
                _cameraInitialPosition.z);
            return;
        }

        if (!_hasLightiningBeenCreated)
        {
            CreateNewLighting();
            _hasLightiningBeenCreated = true;
        }
        
        if (IsFloorRunning && !_hasInitPhaseForTheFirstTime)
        {
            _hasInitPhaseForTheFirstTime = true;
            InitPhase();
        }
    }
    
    private void CreateNewLighting()
    {
        _currentLightingObj = Instantiate(_lightingPrefab, _camera.transform);
    }
    
}
