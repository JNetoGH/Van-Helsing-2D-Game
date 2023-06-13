using System;
using UnityEngine;

public class ThirdFloorManager : MonoBehaviour, IFloorManager
{
    
    // Set by the scene switcher
    public bool IsFloorRunning { get; set; }
    [SerializeField] private Movement _cameraMovement;
    private bool _hasSetPhaseForTheFirstTime = false;
    
    private void InitPhase()
    {
        _cameraMovement.enabled = true;
        _cameraMovement.TeleportToPoint(Movement.TargetPoint.PointA);
        _cameraMovement.ResetWaitingTimer();
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
