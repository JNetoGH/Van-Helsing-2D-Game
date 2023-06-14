using System;
using DataPersistenceSystem;
using TMPro;
using UnityEngine;


public class CheatsManager : MonoBehaviour, IDataPersistenceListener
{
    
    [Header("Ignored (WILL BE OVERRIDEN BY THE DATA PERSISTENCE SYSTEM)")]
    [SerializeField] private bool _isPlayerInvincible;  
    
    [Header("Ignored (WILL BE OVERRIDEN BY THE DATA PERSISTENCE SYSTEM)")]
    [SerializeField] private bool _ignoreFirstFloor;
    [SerializeField] private bool _ignoreSecondFloor;
    [SerializeField] private bool _ignoreThirdFloor;
    [SerializeField] private bool _ignoreFourthFloor;
    
    [Header("Floor Manager Scripts")]
    [SerializeField] private FirstFloorManager _firstFloor;
    [SerializeField] private SecondFloorManager _secondFloor;
    [SerializeField] private ThirdFloorManager _thirdFloor;
    [SerializeField] private FourthFloorManager _fourthFloor;
    
    [Header("Cheats ON msg")]
    [SerializeField] private GameObject _msg; 
    
    private void Start()
    {
        RefreshStates();
       
    }

    private void Update()
    {
        RefreshStates();
    }

    private void RefreshStates()
    {
        // Updates Invincibility 
        PlayerDeathManager.isPlayerInvincible = _isPlayerInvincible;
        
        // Updates Ignore Floor Sequence
        _firstFloor.IgnoreLevel = _ignoreFirstFloor;
        _secondFloor.IgnoreLevel = _ignoreSecondFloor;
        _thirdFloor.IgnoreLevel = _ignoreThirdFloor;
        _fourthFloor.IgnoreLevel = _ignoreFourthFloor;
        
        // Updates the Cheats On msg
        if (_ignoreFirstFloor || _ignoreSecondFloor || _ignoreThirdFloor || _ignoreFourthFloor || _isPlayerInvincible)
            _msg.SetActive(true);
        else
            _msg.SetActive(false);
    }

    public void OnLoadData(GameSerializableData gameSerializableData)
    {
        _isPlayerInvincible = gameSerializableData.invincibility;
        
        _ignoreFirstFloor = gameSerializableData.ignoreFirstFloorSequence;
        _ignoreSecondFloor = gameSerializableData.ignoreSecondFloorSequence;
        _ignoreThirdFloor = gameSerializableData.ignoreThirdFloorSequence;
        _ignoreFourthFloor = gameSerializableData.ignoreFourthFloorSequence;
    }
    
    public void OnSaveData(GameSerializableData gameSerializableData) { }

}
