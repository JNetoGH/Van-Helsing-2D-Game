using System.Collections.Generic;
using PlayerScripts.Enums;
using UnityEngine;

public class MainArmSwitcher : MonoBehaviour
{

    [SerializeField] private ArmState currentArmState = ArmState.Saw;
    [SerializeField] private GameObject crossbowArm;
    [SerializeField] private GameObject sawArm;
    private Dictionary<ArmState, GameObject> stateAndGameObject = new Dictionary<ArmState, GameObject>();
    
    void Start()
    {
        stateAndGameObject.Add(ArmState.Crossbow, crossbowArm);
        stateAndGameObject.Add(ArmState.Saw, sawArm);
    }
    
    void Update()
    {
        foreach (ArmState key in stateAndGameObject.Keys)
            stateAndGameObject[key].SetActive(currentArmState == key);
    }

    public void SwitchArmState(ArmState newState)
    {
        currentArmState = newState;
    }
    
}
