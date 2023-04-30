using System.Collections.Generic;
using PlayerScripts.Enums;
using UnityEngine;

public class MainArmsController : MonoBehaviour
{

    [SerializeField] private ArmState currentArmState = ArmState.Saw;
    [SerializeField] private GameObject crossbowArm;
    [SerializeField] private GameObject sawArm;
    
    private Dictionary<ArmState, GameObject> armStateAndArmGameObject = new Dictionary<ArmState, GameObject>();
    private PlayerController _playerController;
    private GameObject _currentArm;
    
    void Start()
    {
        armStateAndArmGameObject.Add(ArmState.Crossbow, crossbowArm);
        armStateAndArmGameObject.Add(ArmState.Saw, sawArm);
        _playerController = GetComponent<PlayerController>();
        
        SyncCurrentArm();
    }
    
    void Update()
    {
        if (Input.GetButtonDown("ChangeArm"))
        {
            SwitchArms();
            SyncCurrentArm();
        }
        RotateArmAccordingToMousePos();
    }

    private void SyncCurrentArm()
    {
        foreach (ArmState key in armStateAndArmGameObject.Keys)
        {
            armStateAndArmGameObject[key].SetActive(currentArmState == key);
            if (armStateAndArmGameObject[key].activeInHierarchy)
                _currentArm = armStateAndArmGameObject[key];
        }
    }
    
    private void SwitchArms()
    {
        switch (currentArmState)
        {
            case ArmState.Crossbow:
                SetArmState(ArmState.Saw);
                break;
            case ArmState.Saw:
                SetArmState(ArmState.Crossbow);
                break;
        }
    }

    public void SetArmState(ArmState newState)
    {
        currentArmState = newState;
        
        // also makes sure the cooldown timer is 0
        switch (currentArmState)
        {
            case ArmState.Crossbow: CrossbowArmController.ShotCoolDownTimer = 0; break;
            case ArmState.Saw: SawArmController.AtkCoolDownTimer = 0; break;
        }
    }

    private void RotateArmAccordingToMousePos()
    {
        Vector3 mouseInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 distanceToMouse = mouseInWorldPos - _currentArm.transform.position;
        float rotZ = Mathf.Atan2(distanceToMouse.y, distanceToMouse.x) * Mathf.Rad2Deg;
        
        switch (_playerController.CurrentFacingDirection)
        {
            case FacingDirection.Right: _currentArm.transform.rotation = Quaternion.Euler(0f, 0f, rotZ); break;
            case FacingDirection.Left:  _currentArm.transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 180); break;
        }
    }

}
