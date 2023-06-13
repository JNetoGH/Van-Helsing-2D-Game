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
    
    // Ignoring Floor Fields
    private float _defaultOrthoSize;
    private Vector3 _cameraInitialPosition;


    private void Start()
    {
        _defaultOrthoSize = _camera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize;
        _cameraInitialPosition = _camera.transform.position;
    }

    private void InitPhase()
    {
        _cameraMovement.enabled = true;
        _cameraMovement.TeleportToPoint(Movement.TargetPoint.PointA);
        _cameraMovement.ResetWaitingTimer();
        
        // Deletes the old lighting (if possible)
        Destroy(_currentLightingObj);
        Invoke(nameof(CreateNewLighting),0.5f);
    }
    
    // Called by LightningController
    public void OnPlayerDead()
    {
        Debug.Log("Player has died in third floor");
        InitPhase();
        _player.transform.position = _playerRespawnPosition.position;
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

        if (IsFloorRunning && !_hasInitPhaseForTheFirstTime)
        {
            _hasInitPhaseForTheFirstTime = true;
            InitPhase();
        }
    }
    
    private void CreateNewLighting()
    {
        _currentLightingObj = Instantiate(_lightingPrefab, _camera.transform);
        _currentLightingObj.GetComponent<LightningController>().thirdFloorManager = this;
    }
    
}
