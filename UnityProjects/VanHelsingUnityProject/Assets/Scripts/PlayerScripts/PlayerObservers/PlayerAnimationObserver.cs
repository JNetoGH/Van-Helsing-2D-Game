using PlayerScripts.Interfaces;
using UnityEngine;

namespace PlayerScripts.PlayerObservers
{
    public class PlayerAnimationObserver : MonoBehaviour, IPlayerObserver
    {
    
        private Animator _animator;
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public void OnNotifyStart(PlayerController playerController) { }
        public void OnNotifyUpdate(PlayerController playerController)
        {
            // Jump / Fall / Grounded (Idle)
            _animator.SetBool("IsJumping", playerController.IsJumping);
            _animator.SetBool("IsFalling", playerController.IsFalling);
            _animator.SetBool("IsGrounded", playerController.IsGrounded);
            _animator.SetBool("IsWalking", playerController.IsMoving);
        }
    
    }
}
