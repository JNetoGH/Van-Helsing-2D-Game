using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class FourthFloorManager : MonoBehaviour, IFloorManager
{
    
    // Set by the scene switcher
    public bool IsFloorRunning { get; set; }

    // Set by the Level Ignorer Script
    public bool IgnoreLevel { get; set; }

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
    
    [Header("Werewolf Unlocked Msg")]
    [SerializeField] private AscendAndDisappearText _werewolfUnlockedMsg;
    
    [Header("Werewolf Cooldown Msg")]
    [SerializeField] private AscendAndDisappearText _cooldownText;
    [SerializeField] private Transform _msgInstantiationWorldPos;

    [Header("Horde Spawning Dependencies")]
    [SerializeField] private float _spawnRateInSec = 1; 
    [SerializeField] private GameObject _hordePrefab3;
    [SerializeField] private GameObject _hordePrefab4;
    [SerializeField] private GameObject _hordePrefab6;
    [SerializeField] private List<Transform> _spawnPoints;
    private int _curSpawnPoint;
    private float _spawnTimer;
    private GameObject _currentHordePrefab;
    
    // Werewolf trigger cooldown
    public const float WerewolfCooldownDurationInSec = 8;
    public float WerewolfCooldownTimer { get; private set; }

    public void OnPlayerDead()
    {
        ResetPhase();
        _player.transform.position = _playerRespawnPosition.position;
    }

    // called by the Enemy script on the OnDeath event or if the ignoreFloor is true
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
       
        // Resets the werewolf cooldown timer
        WerewolfCooldownTimer = 0;
    }
    
    public void InitPhase()
    {
        // Destroys the previous dracula if had any
        if (_currentDracula is not null)
            Destroy(_currentDracula);
        
        // Level ignoring validation
        if (IgnoreLevel)
            EndSequence();
        
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
        
        // Resets the werewolf cooldown timer
        WerewolfCooldownTimer = 0;
        
        // Werewolf unlocked msg
        _werewolfUnlockedMsg.InstantiateMsgAtScreenMiddle();
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
        
        // werewolf super move trigger
        WerewolfCooldownTimer -= Time.deltaTime;
        bool canTriggerWerewolf = WerewolfCooldownTimer <= 0;
        if (canTriggerWerewolf)
        {
            // triggers the werewolf and resets the cooldown
            if (Input.GetMouseButtonDown(1))
            {
                _player.GetComponent<Animator>().SetTrigger("WerewolfSuperMove");
                WerewolfCooldownTimer = WerewolfCooldownDurationInSec;
            }
        }
        else
        {
            // in cooldown msg
            if (Input.GetMouseButtonDown(1))
            {
                var msgPos = _msgInstantiationWorldPos.transform.position;
                _cooldownText.InstantiateMsgWorldPosition(msgPos);
            }
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
