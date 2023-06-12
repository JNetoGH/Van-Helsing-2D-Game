using System;
using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera _camA;
    [SerializeField] private CinemachineVirtualCamera _camB;
    [SerializeField] private PlayerController _playerController;
    public bool HasSwitched { get; set; } = false;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        // Validation Gate Way
        if (!(other.gameObject == _playerController.gameObject && _playerController.IsGrounded && !HasSwitched)) 
            return;
        
        Debug.Log("Player is in a Camara Switcher");
        _camA.enabled = false;
        _camB.enabled = true;
        HasSwitched = true;
    }
}
