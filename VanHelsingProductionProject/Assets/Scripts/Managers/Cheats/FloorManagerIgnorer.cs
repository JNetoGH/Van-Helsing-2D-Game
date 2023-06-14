using System;
using DataPersistenceSystem;
using UnityEngine;


public class FloorManagerIgnorer : MonoBehaviour, IDataPersistenceListener
{
    
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
        _firstFloor.IgnoreLevel = _ignoreFirstFloor;
        _secondFloor.IgnoreLevel = _ignoreSecondFloor;
        _thirdFloor.IgnoreLevel = _ignoreThirdFloor;
        _fourthFloor.IgnoreLevel = _ignoreFourthFloor;
    }

    public void OnLoadData(GameSerializableData gameSerializableData)
    {
        _ignoreFirstFloor = gameSerializableData.ignoreFirstFloorSequence;
        _ignoreSecondFloor = gameSerializableData.ignoreSecondFloorSequence;
        _ignoreThirdFloor = gameSerializableData.ignoreThirdFloorSequence;
        _ignoreFourthFloor = gameSerializableData.ignoreFourthFloorSequence;
    }
    
    public void OnSaveData(GameSerializableData gameSerializableData) { }
    
}
