using System.Collections.Generic;
using PlayerScripts.Enums;
using PlayerScripts.Interfaces;
using UnityEngine;

public class PlayerArmsHandlerObserver : MonoBehaviour, IPlayerObserver
{
    
    // Switch System
    [Header("Arm Switching System")]
    [SerializeField] private ArmState _currentArmState = ArmState.Saw;
    [SerializeField] private GameObject _crossbowArm;
    [SerializeField] private GameObject _sawArm;
    private GameObject _currentArm;
    private readonly Dictionary<ArmState, GameObject> _armStateAndArmGameObject = new();
    
    // Arm controllers
    private SawArmController _sawArmController;
    private CrossbowArmController _crossbowArmController;
    
    public void OnNotifyStart(PlayerController playerController)
    {
        _armStateAndArmGameObject.Add(ArmState.Crossbow, _crossbowArm);
        _armStateAndArmGameObject.Add(ArmState.Saw, _sawArm);
        _crossbowArmController = _crossbowArm.GetComponent<CrossbowArmController>();
        _sawArmController = _sawArm.GetComponent<SawArmController>();
        SetArm(_currentArmState);
    }
    
    public void OnNotifyUpdate(PlayerController playerController)
    {
        if (Input.GetButtonDown("ChangeArm"))
            SwitchArms();
        RotateArmAccordingToMousePos(playerController);
        UpdateCooldown();
    }
    
    private void SwitchArms()
    {
        SetArm(_currentArmState == ArmState.Saw ? ArmState.Crossbow : ArmState.Saw);
    }

    public void SetArm(ArmState newState)
    {
        // Changes the _currentArmState
        _currentArmState = newState;
        
        // Updates the dictionary and sets the _currentArm based on it
        foreach (ArmState key in _armStateAndArmGameObject.Keys)
        {
            _armStateAndArmGameObject[key].SetActive(key == _currentArmState);
            if (_armStateAndArmGameObject[key].activeInHierarchy)
                _currentArm = _armStateAndArmGameObject[key];
        }
    }
    
    private void RotateArmAccordingToMousePos(PlayerController playerController)
    {
        Vector3 mouseInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 distanceToMouse = mouseInWorldPos - _currentArm.transform.position;
        float rotZ = Mathf.Atan2(distanceToMouse.y, distanceToMouse.x) * Mathf.Rad2Deg;
        switch (playerController.CurrentFacingDirection)
        {
            case FacingDirection.Right: _currentArm.transform.rotation = Quaternion.Euler(0f, 0f, rotZ); break;
            case FacingDirection.Left:  _currentArm.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 180); break;
        }
    }
    
    private void UpdateCooldown()
    {
        // Updates the arms timer: required to be out oof the arm controller because they're deactivated whe switched
        _sawArmController.AtkCooldownTimer -= Time.deltaTime;
        _crossbowArmController.ShotCooldownTimer -= Time.deltaTime;

        // Keeps the min cooldown at 0: has to be zero in order to sync with the GUI slider goes from Duration to 0
        if (_sawArmController.AtkCooldownTimer < 0)
            _sawArmController.AtkCooldownTimer = 0;
        if (_crossbowArmController.ShotCooldownTimer < 0)
            _crossbowArmController.ShotCooldownTimer = 0;
    }
    
}
