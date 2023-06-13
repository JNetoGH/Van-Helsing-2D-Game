using UnityEngine;

public class ThirdFloorManager : MonoBehaviour, IFloorManager
{
    
    // Set by the scene switcher
    public bool IsFloorRunning { get; set; }
    
    [SerializeField] private Movement _cameraMovement;
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _lightingPrefab;

    // Controlll
    private GameObject _currentLightingObj;
    private bool _hasSetPhaseForTheFirstTime = false;
    
    private void InitPhase()
    {
        _cameraMovement.enabled = true;
        _cameraMovement.TeleportToPoint(Movement.TargetPoint.PointA);
        _cameraMovement.ResetWaitingTimer();
        
        // Deletes the old lighting (if possible)
        Destroy(_currentLightingObj);
        _currentLightingObj = Instantiate(_lightingPrefab, _camera.transform);
    }
    
    public void OnPlayerDead()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (IsFloorRunning && !_hasSetPhaseForTheFirstTime)
        {
            _hasSetPhaseForTheFirstTime = true;
            InitPhase();
        }
    }
    
}
