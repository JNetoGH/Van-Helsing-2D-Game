using System.Collections.Generic;
using PlayerScripts.Enums;
using UnityEngine;

public class MainArmsController : MonoBehaviour
{

    [SerializeField] private ArmState currentArmState = ArmState.Saw;
    [SerializeField] private GameObject crossbowArm;
    [SerializeField] private GameObject sawArm;
    
    private Dictionary<ArmState, GameObject> stateAndGameObject = new Dictionary<ArmState, GameObject>();
    private PlayerController _playerController;
    
    void Start()
    {
        stateAndGameObject.Add(ArmState.Crossbow, crossbowArm);
        stateAndGameObject.Add(ArmState.Saw, sawArm);
        _playerController = GetComponentInParent<PlayerController>();
        
        // Making sure that the angle in 0 is zero
        transform.rotation = Quaternion.Euler(0f, 0f, 0f); 
    }
    
    void Update()
    {
        foreach (ArmState key in stateAndGameObject.Keys)
            stateAndGameObject[key].SetActive(currentArmState == key);
        
        if (Input.GetButtonDown("ChangeArm"))
        {
            switch (currentArmState)
            {
                case ArmState.Crossbow: SwitchArmState(ArmState.Saw); break;
                case ArmState.Saw: SwitchArmState(ArmState.Crossbow); break;
            }
        }
        
        RotateArmAccordingToMousePos();
    }

    public void SwitchArmState(ArmState newState)
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
        Vector3 distanceToMouse = mouseInWorldPos - transform.position;
        float rotZ = Mathf.Atan2(distanceToMouse.y, distanceToMouse.x) * Mathf.Rad2Deg;
        
        switch (_playerController.CurrentFacingDirection)
        {
            case FacingDirection.Right: transform.rotation = Quaternion.Euler(0f, 0f, rotZ); break;
            case FacingDirection.Left:  transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 180); break;
        }
    }

}
