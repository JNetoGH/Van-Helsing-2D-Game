using PlayerScripts.Interfaces;
using UnityEngine;

namespace PlayerScripts.PlayerObservers
{
    public class PlayerAnimationObserver : MonoBehaviour, IPlayerObserver
    {
    
        [SerializeField] private Animator spritesAnimator;
        private Rigidbody2D _rb;

        private void Start() => _rb = GetComponent<Rigidbody2D>();

        public void OnNotifyStart(PlayerController playerController) { }

        public void OnNotifyUpdate(PlayerController playerController)
        {
            // Run
            if (Mathf.Abs(_rb.velocity.x) > 0f && Mathf.Abs(_rb.velocity.x) > playerController.maxWalkSpeed) 
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
}
