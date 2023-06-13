using System;
using System.Collections.Generic;
using UnityEngine;


public class FloorManagerIgnorer : MonoBehaviour
{
    
    [Header("Ignored (DO NOT SHIP TURNED ON)")]
    [SerializeField] private bool _ignoreFirstFloor;
    [SerializeField] private bool _ignoreSecondFloor;
    [SerializeField] private bool _ignoreThirdFloor;
    
    [Header("Floor Manager Scripts")]
    [SerializeField] private FirstFloorManager _firstFloor;
    [SerializeField] private SecondFloorManager _secondFloor;
    [SerializeField] private ThirdFloorManager _thirdFloor;

    private void Start()
    {
        _firstFloor.IgnoreLevel = _ignoreFirstFloor;
        _secondFloor.IgnoreLevel = _ignoreSecondFloor;
    }
    
}
