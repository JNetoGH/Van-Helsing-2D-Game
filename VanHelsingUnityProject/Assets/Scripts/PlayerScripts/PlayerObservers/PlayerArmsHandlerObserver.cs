using System.Collections.Generic;
using PlayerScripts.Enums;
using PlayerScripts.Interfaces;
using UnityEngine;

public class PlayerArmsHandlerObserver : MonoBehaviour, IPlayerObserver
{

    [Header("Arm Switching System")]
    [SerializeField] private ArmState _currentArmState = ArmState.Saw;
    [SerializeField] private GameObject _crossbowArm;
    [SerializeField] private GameObject _sawArm;
    
    private GameObject _currentArm;
    private readonly Dictionary<ArmState, GameObject> _armStateAndArmGameObject = new();
    
    public void OnNotifyStart(PlayerController playerController)
    {
        _armStateAndArmGameObject.Add(ArmState.Crossbow, _crossbowArm);
        _armStateAndArmGameObject.Add(ArmState.Saw, _sawArm);
        SetArm(_currentArmState);
    }
    
    public void OnNotifyUpdate(PlayerController playerController)
    {
        if (Input.GetButtonDown("ChangeArm"))
            SwitchArms();
        RotateArmAccordingToMousePos(playerController);
    }
    
    private void SwitchArms()
    {
        SetArm(_currentArmState == ArmState.Saw ? ArmState.Crossbow : ArmState.Saw);
    }

    public void SetArm(ArmState newState)
    {
        // Changes the _currentArmState
        _currentArmState = newState;
        switch (_currentArmState)
        {   // also makes sure to reset the cooldown
            case ArmState.Crossbow: CrossbowArmController.ShotCoolDownTimer = 0; break;
            case ArmState.Saw: SawArmController.AtkCoolDownTimer = 0; break;
        }
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

}
