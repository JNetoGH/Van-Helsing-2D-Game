using Cinemachine;
using UnityEngine;

public class FirstFloorManager : MonoBehaviour, IFloorManager
{
    
    // Set by the scene switcher
    public bool IsFloorRunning { get; set; }

    // Set by the Level Ignorer Script
    public bool IgnoreLevel { get; set; }
    
    [Header("Cameras Settings")]
    [SerializeField] private CinemachineVirtualCamera _cam0;
    [SerializeField] private CinemachineVirtualCamera _cam1;

    [Header("Player Dependencies")]
    [SerializeField] private PlayerController _playerController;
    
    [Header("Level Init Timer Settings")]
    [SerializeField] private float _levelWaitingDuration = 3.5f;
    private float _timer;
    
    
    // Comes from the Interface, called by LightningController
    public void OnPlayerDead()
    {
        
    }

    public void InitPhase()
    {
        _cam0.enabled = true;
        _cam1.enabled = false;
        IsFloorRunning = true;
        _playerController.canMove = false;
        _timer = _levelWaitingDuration;
        PlayerDeathManager.currentFloorManager = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        InitPhase();
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (!IsFloorRunning)
            return;

        // Ignoring Floor Implementation
        if (IgnoreLevel)
        {
            _cam0.enabled = false;
            _cam1.enabled = true;
            _playerController.canMove = true;
            return;
        }
        
        // makes the scene wait a bit before allowing the player to move
        // and change the camera
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer > 0)
                return;
            _playerController.canMove = true;
            _cam0.enabled = false;
            _cam1.enabled = true;
        }
        
    }
    
    
    
}
