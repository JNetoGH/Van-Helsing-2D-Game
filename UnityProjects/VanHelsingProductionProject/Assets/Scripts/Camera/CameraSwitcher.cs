using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CameraSwitcher : MonoBehaviour
{

    [Header("Cameras Settings")]
    [SerializeField] private CinemachineVirtualCamera _camA;
    [SerializeField] private CinemachineVirtualCamera _camB;
    
    [Header("Player Settings")]
    [SerializeField] private PlayerController _playerController;
    
    [Header("Event Settings")]
    [SerializeField] private UnityEvent _onCameraSwitch;
    
    // Control Property
    public bool HasSwitched { get; private set; } = false;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        // Validation Gate Way
        if (!(other.gameObject == _playerController.gameObject && _playerController.IsGrounded && !HasSwitched)) 
            return;
        
        Debug.Log("Player is in a Camara Switcher");
        _camA.enabled = false;
        _camB.enabled = true;
        HasSwitched = true;
        
        // Makes player stop moving at Animator
        Animator playerAnimator = _playerController.gameObject.GetComponent<Animator>();
        playerAnimator.SetBool("IsGrounded", true);
        playerAnimator.SetBool("IsWalking", false);
        playerAnimator.Play("Idle");
        
        // Makes player stop moving at Rigidbody
        Rigidbody2D playerRigidbody = _playerController.gameObject.GetComponent<Rigidbody2D>();
        Vector3 curVel = playerRigidbody.velocity;
        Vector3 newVel = new Vector3(0, curVel.x, 0);
        playerRigidbody.velocity = newVel;
        
        // invokes the events
        _onCameraSwitch.Invoke();
    }
}
