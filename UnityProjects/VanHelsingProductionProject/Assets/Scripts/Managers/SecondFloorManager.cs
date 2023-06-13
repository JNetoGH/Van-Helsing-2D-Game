using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


public class SecondFloorManager : MonoBehaviour, IFloorManager
{
    
    // Set by the scene switcher
    public bool IsFloorRunning { get; set; }

    // Set by the Level Ignorer Script
    public bool IgnoreLevel { get; set; }

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
    
    [Header("PLayer Respawn Settings")]
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _playerRespawnPosition;
    
    [Header("Horde Spawning Dependencies")]
    [SerializeField] private GameObject _hordePrefab;
    [SerializeField] private List<Transform> _spawnPoints;
    private float _spawnRateInSec; // changed in order to change the difficulty
    private int _curSpawnPoint;
    private float _spawnTimer;
    private bool _hasFinishedSpawning;

    // End Sequence Controlling
    private bool _initEndSequence;
    private const float StartEndSequenceDelayInSec = 2;
    
    // Comes from the Interface, called by LightningController
    public void OnPlayerDead()
    {
        Debug.LogWarning("Player died on level 2");
        
        // Teleports player and kills its velocity
        _player.transform.position = _playerRespawnPosition.position;
        _player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        
        // Also brute forces the animation to default idle
        Animator playerAnimator = _player.GetComponent<Animator>();
        playerAnimator.Play("Idle");
        
        // removes every enemy
        Array.ForEach(GameObject.FindGameObjectsWithTag("Enemy"), e => Destroy(e));
        
        // Reset internal stuff
        InitPhase();
        _waitTimer = _levelWaitingDuration;
    }
    
    public void InitPhase()
    {
        _playerController.canMove = true;
        _spawnTimer = 0;
        _curSpawnPoint = 0;
        _spawnRateInSec = 3f;
        _hasFinishedSpawning = false;
        _initEndSequence = false;
        PlayerDeathManager.currentFloorManager = this;
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
                return;
            
            //  in case the init timer has finished counting
            InitPhase();
        }

        // Ignoring Floor Implementation
        if (IgnoreLevel)
        {
            _hasFinishedSpawning = true;
            _initEndSequence = true;
            UpdateEndSequence();
            return;
        }    
        
        // Update
        SpawnHorde();
        bool areAllEnemiesDead = GameObject.FindWithTag("Enemy") is null;
        if (areAllEnemiesDead)
            UpdateEndSequence();
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
    
}
