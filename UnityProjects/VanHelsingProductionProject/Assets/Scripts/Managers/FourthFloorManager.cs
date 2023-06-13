using System;
using System.Collections.Generic;
using UnityEngine;

public class FourthFloorManager : MonoBehaviour, IFloorManager
{
    
    public bool IsFloorRunning { get; set; }

    [Header("Level Init Timer Settings")]
    [SerializeField] private float _levelWaitingDuration = 5f;
    private float _waitTimer;
    
    [Header("Dracula Settings")]
    [SerializeField] private GameObject _draculaPrefab;
    [SerializeField] private DraculaHPBarController _draculaHpBarController;
    private GameObject _currentDracula;
    private Enemy _draculaEnemyScript;
    
    [Header("PLayer Respawn Settings")]
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _playerRespawnPosition;

    [Header("Winning Msg")]
    [SerializeField] private AscendAndDisappearText _winningMsg;

    [Header("Horde Spawning Dependencies")]
    [SerializeField] private float _spawnRateInSec = 1; 
    [SerializeField] private GameObject _hordePrefab3;
    [SerializeField] private GameObject _hordePrefab4;
    [SerializeField] private GameObject _hordePrefab6;
    [SerializeField] private List<Transform> _spawnPoints;
    private int _curSpawnPoint;
    private float _spawnTimer;
    private GameObject _currentHordePrefab;

    public void OnPlayerDead()
    {
        ResetPhase();
        _player.transform.position = _playerRespawnPosition.position;
    }

    // called by the Enemy script on the OnDeath event
    public void EndSequence()
    {
        Debug.LogWarning($"Player killed dracula");
        IsFloorRunning = false;
        
        // removes every enemy
        Array.ForEach(GameObject.FindGameObjectsWithTag("Enemy"), e => Destroy(e));
        _player.GetComponent<PlayerController>().canMove = false;
        _winningMsg.InstantiateMsgAtScreenMiddle();
    }

    private void ResetPhase()
    {
        _waitTimer = _levelWaitingDuration;
        // removes every enemy
        Array.ForEach(GameObject.FindGameObjectsWithTag("Enemy"), e => Destroy(e));
    }
    
    public void InitPhase()
    {
        // Destroys the previous dracula if had any
        if (_currentDracula is not null)
            Destroy(_currentDracula);
        
        // removes every enemy
        Array.ForEach(GameObject.FindGameObjectsWithTag("Enemy"), e => Destroy(e));
        
        // Creates a new Dracula
        _currentDracula = Instantiate(_draculaPrefab);
        _draculaEnemyScript = _currentDracula.GetComponentInChildren<Enemy>();
        
        // Dracula event setting
        _draculaEnemyScript.onDeath.RemoveAllListeners();
        _draculaEnemyScript.onDeath.AddListener(EndSequence);

        // Syncs the HP bar with this new one
        _draculaHpBarController.Dracula = _draculaEnemyScript;

        // Horde Settings
        _spawnTimer = 0;
        _curSpawnPoint = 0;
        _currentHordePrefab = _hordePrefab3;
        PlayerDeathManager.currentFloorManager = this;
    }
    
    private void Start()
    {
        ResetPhase();
        _currentHordePrefab = _hordePrefab3;
    }
    
    void Update()
    {
        if (!IsFloorRunning)
            return;
        
        // makes the scene wait a bit before allowing the player to move
        if (_waitTimer > 0)
        {
            _waitTimer -= Time.deltaTime;
        
            // in case the init timer hasn't finished
            if (_waitTimer > 0) return;
        
            //  in case the init timer has finished counting
            InitPhase();
        }
        
        
        
        // Test werewolf super move
        if (Input.GetKeyDown(KeyCode.O))
        {
            _player.GetComponent<Animator>().SetTrigger("WerewolfSuperMove");
        }
        
        // Phase sequence
        if (_currentDracula is not null)
        {
            TrySpawnHorde();
        }

    }

    private void TrySpawnHorde()
    {
        
        bool canSpawnHorde = _spawnTimer <= 0 && _currentDracula is not null;
        
        // Updates the spawn timer
        if (canSpawnHorde) _spawnTimer = _spawnRateInSec;
        else _spawnTimer -= Time.deltaTime;

        // spawn the horde
        if (!canSpawnHorde) return;
        
        Debug.LogWarning("Horde Spawned");
        
        // progressive difficulty
        // _currentHordePrefab = _currentDracula.GetComponent<Enemy>().HealthPoints switch
        // {
        //     > 40 => _hordePrefab3, 
        //     > 25 => _hordePrefab4,
        //     _ => _currentHordePrefab
        // };

        // spawn
        GameObject horde = Instantiate(_currentHordePrefab);
        horde.transform.position = _spawnPoints[_curSpawnPoint].position;
        _curSpawnPoint++;

        // reset the spawn points
        if (_curSpawnPoint >= _spawnPoints.Count)
            _curSpawnPoint = 0;
    }
    
}
