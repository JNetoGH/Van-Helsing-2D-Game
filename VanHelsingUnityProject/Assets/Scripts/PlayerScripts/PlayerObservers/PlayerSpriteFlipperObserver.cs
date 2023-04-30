using PlayerScripts.Enums;
using PlayerScripts.Interfaces;
using UnityEngine;

namespace PlayerScripts.PlayerObservers
{
    public class PlayerSpriteFlipperObserver : MonoBehaviour, IPlayerObserver
    {

        [SerializeField] private FacingDirection _spriteDefaultFacingDirection = FacingDirection.Right;
        private Vector3 _facingRightScale;
        private Vector3 _facingLeftScale;

        private void Start()
        {
            // scale for sprite sync
            Vector3 currentScale = transform.localScale;
            _facingRightScale = currentScale;
            _facingLeftScale = currentScale;
            switch (_spriteDefaultFacingDirection)
            {
                case FacingDirection.Right: _facingLeftScale.x *= -1; break;
                case FacingDirection.Left: _facingRightScale.x *= -1; break;
            }
        }

        public void OnNotifyStart(PlayerController playerController) { }
        public void OnNotifyUpdate(PlayerController playerController)
        {
            // Updates sprite in X axis scale, in order to flip the sprite to the current facing direction
            if (playerController.CurrentFacingDirection == FacingDirection.Left) 
                transform.localScale = _facingLeftScale;
            else if (playerController.CurrentFacingDirection == FacingDirection.Right) 
                transform.localScale = _facingRightScale;
        }
        
    }
}
