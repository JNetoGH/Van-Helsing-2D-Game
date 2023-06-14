using System;
using DataPersistenceSystem;
using UnityEngine;
using UnityEngine.UI;

public class CheatsMenuController : MonoBehaviour, IDataPersistenceListener
{

    [Header("Van Helsing")]
    [SerializeField] private Toggle _invincibilityToggle;

    [Header("Ignore Floor Sequence")]    
    [SerializeField] private Toggle _ignoreFirstFloorSequenceToggle;
    [SerializeField] private Toggle _ignoreSecondFloorSequenceToggle;
    [SerializeField] private Toggle _ignoreThirdFloorSequenceToggle;
    [SerializeField] private Toggle _ignoreFourthFloorSequenceToggle;

    public void SaveToggleButtonStatesToJsonFIle()
    {
        DataPersistenceManager.Instance.GameSerializableData.invincibility = _invincibilityToggle.isOn;
        DataPersistenceManager.Instance.GameSerializableData.ignoreFirstFloorSequence = _ignoreFirstFloorSequenceToggle.isOn;
        DataPersistenceManager.Instance.GameSerializableData.ignoreSecondFloorSequence = _ignoreSecondFloorSequenceToggle.isOn;
        DataPersistenceManager.Instance.GameSerializableData.ignoreThirdFloorSequence = _ignoreThirdFloorSequenceToggle.isOn;
        DataPersistenceManager.Instance.GameSerializableData.ignoreFourthFloorSequence = _ignoreFourthFloorSequenceToggle.isOn;
        DataPersistenceManager.Instance.SaveData();
    }
    
    public void OnLoadData(GameSerializableData gameSerializableData)
    {
        _invincibilityToggle.isOn = DataPersistenceManager.Instance.GameSerializableData.invincibility;
        _ignoreFirstFloorSequenceToggle.isOn = DataPersistenceManager.Instance.GameSerializableData.ignoreFirstFloorSequence;
        _ignoreSecondFloorSequenceToggle.isOn = DataPersistenceManager.Instance.GameSerializableData.ignoreSecondFloorSequence;
        _ignoreThirdFloorSequenceToggle.isOn = DataPersistenceManager.Instance.GameSerializableData.ignoreThirdFloorSequence;
        _ignoreFourthFloorSequenceToggle.isOn = DataPersistenceManager.Instance.GameSerializableData.ignoreFourthFloorSequence;
    }

    public void OnSaveData(GameSerializableData gameSerializableData) { }
    
}
