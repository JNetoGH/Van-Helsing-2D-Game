using Interfaces;
using UnityEngine;

public class PlayerAnimationObserver : MonoBehaviour, IPlayerObserver
{
    
    [SerializeField] private Animator spritesAnimator;
    private Rigidbody2D _rb;
    
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void OnNotify(PlayerController playerController)
    {
        // walk, run and idle
        
        // Run
        if (Mathf.Abs(_rb.velocity.x) > 0f && Mathf.Abs(_rb.velocity.x) > PlayerController.MaxWalkSpeed) 
        {
            spritesAnimator.SetBool("IsRunning", true);
            spritesAnimator.SetBool("IsWalking", false);
            spritesAnimator.SetBool("IsInIdle", false);
        }
        // Walk
        else if (Mathf.Abs(_rb.velocity.x) > 0f) 
        {
            spritesAnimator.SetBool("IsRunning", false);
            spritesAnimator.SetBool("IsWalking", true);
            spritesAnimator.SetBool("IsInIdle", false);
        }
        // Idle
        else 
        {
            spritesAnimator.SetBool("IsRunning", false);
            spritesAnimator.SetBool("IsWalking", false);
            spritesAnimator.SetBool("IsInIdle", true);
        }
    }
}
