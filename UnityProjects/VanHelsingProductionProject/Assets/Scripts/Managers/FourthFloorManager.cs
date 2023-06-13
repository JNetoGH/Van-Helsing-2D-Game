using System;
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

    public void OnPlayerDead()
    {
        ResetPhase();
    }

    private void ResetPhase()
    {
        _waitTimer = _levelWaitingDuration;
    }
    
    public void InitPhase()
    {
        // Destroys the previous dracula if had any
        if (_currentDracula is not null)
            Destroy(_currentDracula);
        
        // removes every enemy
        Array.ForEach(GameObject.FindGameObjectsWithTag("Enemy"), e => Destroy(e));
        
        // Creates a new one
        _currentDracula = Instantiate(_draculaPrefab);

        // Syncs the HP bar with this new one
        _draculaHpBarController.Dracula = _currentDracula.GetComponentInChildren<Enemy>();
    }
    
    private void Start()
    {
        ResetPhase();
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
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            ResetPhase();
            InitPhase();
        }
        
        // Phase sequence
        if (_currentDracula is null)
            Debug.LogWarning($"Player killed dracula");
    }
    
}
