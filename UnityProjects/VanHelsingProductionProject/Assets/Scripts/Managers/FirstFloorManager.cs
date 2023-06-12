using Cinemachine;
using UnityEngine;

public class FirstFloorManager : MonoBehaviour, IFloorManager
{
    
    public bool IsFloorRunning { get; set; }
    
    [Header("Cameras Settings")]
    [SerializeField] private CinemachineVirtualCamera _cam0;
    [SerializeField] private CinemachineVirtualCamera _cam1;

    [Header("Player Dependencies")]
    [SerializeField] private PlayerController _playerController;
    
    [Header("Level Init Timer Settings")]
    [SerializeField] private float _levelWaitingDuration = 3.5f;
    private float _timer;
    
    public void RespawnPlayer()
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _cam0.enabled = true;
        _cam1.enabled = false;
        IsFloorRunning = true;
        _playerController.canMove = false;
        _timer = _levelWaitingDuration;
    }
    
    // Update is called once per frame
    void Update()
    {
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
