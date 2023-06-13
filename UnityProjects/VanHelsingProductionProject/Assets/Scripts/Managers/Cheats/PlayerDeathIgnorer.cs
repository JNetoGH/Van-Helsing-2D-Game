using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerDeathIgnorer : MonoBehaviour
{

    [SerializeField] private bool _isPlayerInvincible;
    
    private void Start()
    {
        PlayerDeathManager.isPlayerInvincible = _isPlayerInvincible;
    }

    private void Update()
    {
        PlayerDeathManager.isPlayerInvincible = _isPlayerInvincible;
    }
    
}
