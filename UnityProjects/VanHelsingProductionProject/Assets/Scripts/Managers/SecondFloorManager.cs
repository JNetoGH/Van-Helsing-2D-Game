using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Animations;
using UnityEngine;

public class SecondFloorManager : MonoBehaviour, IFloorManager
{
    
    // Set by the scene switcher
    public bool IsFloorRunning { get; set; }

    [Header("Floor Switching Dependencies")]
    [SerializeField] private FirstFloorManager _lastFloor;
    
    [Header("Player Dependencies")]
    [SerializeField] private PlayerController _playerController;
    
    [Header("Level Init Timer Settings")]
    [SerializeField] private float _levelWaitingDuration = 1f;
    private float _waitTimer;

    [Header("Finish Sequence Dependencies")]
    [SerializeField] private GameObject _secondFloorPlatforms;
    private const float SecondFloorPlatformsSpeed = 4;
    
    [Header("Cameras Settings")]
    [SerializeField] private CinemachineVirtualCamera _cam2;
    [SerializeField] private CinemachineVirtualCamera _cam3;
    
    [Header("Horde Spawning Dependencies")]
    [SerializeField] private GameObject _hordePrefab;
    [SerializeField] private List<Transform> _spawnPoints;
    private float _spawnRateInSec; // changed in order to change the diffculty
    private int _curSpawnPoint;
    private float _spawnTimer;
    private bool _hasFinishedSpawning;

    // End Sequence Conmtrolling
    private bool _initEndSequence;
    private const float StartEndSequenceDelayInSec = 8;
    
    // Comes from the Interface
    public void OnPlayerDead()
    {
        
    }
    
    private void InitPhase()
    {
        _playerController.canMove = true;
        _spawnTimer = 0;
        _curSpawnPoint = 0;
        _spawnRateInSec = 3f;
        _hasFinishedSpawning = false;
        _initEndSequence = false;
    }
    
    private void InitEndSequenceCall()
    {
        _initEndSequence = true;
    }
    
    private void Start()
    {
        _waitTimer = _levelWaitingDuration;
        IsFloorRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsFloorRunning)
            return;
        
        // makes the scene wait a bit before allowing the player to move
        if (_waitTimer > 0)
        {
            _waitTimer -= Time.deltaTime;
            
            // in case the init timer hasn't finished
            if (_waitTimer > 0)
            {
                _playerController.canMove = false;
                return;
            }
            
            //  in case the init timer has finished counting
            InitPhase();
        }
        
        SpawnHorde();
        UpdateEndSequence();
    }

    private void UpdateEndSequence()
    {
        // Checks if the End Sequence should be initiated or not
        if (_hasFinishedSpawning && !_initEndSequence)
        {
            Invoke(nameof(InitEndSequenceCall), StartEndSequenceDelayInSec);
            Debug.LogWarning("Player finished floor 2");
        }

        // Updates the End Sequence
        if (_initEndSequence)
        {
            // switches the current Virtual Cam
            _cam2.enabled = false;
            _cam3.enabled = true;
            
            // Moves the platforms
            _secondFloorPlatforms.transform.position = Vector3.MoveTowards(
                current: _secondFloorPlatforms.transform.position,
                target: new Vector3(-1, _secondFloorPlatforms.transform.position.y, 0),
                maxDistanceDelta: SecondFloorPlatformsSpeed * Time.deltaTime
            );
            
            
        }
    }

    private void SpawnHorde()
    {
        // Updates the spawn timer
        bool canSpawnHorde = _spawnTimer <= 0;
        if (canSpawnHorde) _spawnTimer = _spawnRateInSec;
        else _spawnTimer -= Time.deltaTime;

        // spawn the horde
        if (canSpawnHorde && !_hasFinishedSpawning)
        {
            // spawn
            GameObject horde = Instantiate(_hordePrefab);
            horde.transform.position = _spawnPoints[_curSpawnPoint].position;
            _curSpawnPoint++;

            // progressive difficulty
            if (_curSpawnPoint == 4)
                _spawnRateInSec = 2.5f;
            if (_curSpawnPoint == 7)
                _spawnRateInSec = 2f;

            // quite the spawning
            if (_curSpawnPoint >= _spawnPoints.Count)
                _hasFinishedSpawning = true;
        }
    }
}
