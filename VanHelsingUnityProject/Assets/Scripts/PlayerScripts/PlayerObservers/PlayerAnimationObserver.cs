using PlayerScripts.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerScripts.PlayerObservers
{
    public class PlayerAnimationObserver : MonoBehaviour, IPlayerObserver
    {
    
        [SerializeField] private Animator _spritesAnimator;
        private Rigidbody2D _rb;

        private void Start() => _rb = GetComponent<Rigidbody2D>();
        public void OnNotifyStart(PlayerController playerController) { }
        public void OnNotifyUpdate(PlayerController playerController)
        {
            /*// Run
            if (Mathf.Abs(_rb.velocity.x) > 0f && Mathf.Abs(_rb.velocity.x) > playerController.maxWalkSpeed) 
            {
                _spritesAnimator.SetBool("IsRunning", true);
                _spritesAnimator.SetBool("IsWalking", false);
                _spritesAnimator.SetBool("IsInIdle", false);
            }
            // Walk
            else if (Mathf.Abs(_rb.velocity.x) > 0f) 
            {
                _spritesAnimator.SetBool("IsRunning", false);
                _spritesAnimator.SetBool("IsWalking", true);
                _spritesAnimator.SetBool("IsInIdle", false);
            }
            // Idle
            else 
            {
                _spritesAnimator.SetBool("IsRunning", false);
                _spritesAnimator.SetBool("IsWalking", false);
                _spritesAnimator.SetBool("IsInIdle", true);
            }*/
            
            //_animator.SetBool("Grounded", IsGrounded);
        }
    
    }
}
