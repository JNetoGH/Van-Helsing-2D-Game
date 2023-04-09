using PlayerScripts.Enums;
using PlayerScripts.Interfaces;
using UnityEngine;

namespace PlayerScripts.PlayerObservers
{
    public class PlayerSpriteFlippersObserver : MonoBehaviour, IPlayerObserver
    {

        [SerializeField] private FacingDirection _spriteDefaultFacingDirection = FacingDirection.Right;
        private Vector3 facingRightScale;
        private Vector3 facingLeftScale;

        private void Start()
        {
            // scale for sprite sync
            Vector3 currentScale = transform.localScale;
            facingRightScale = currentScale;
            facingLeftScale = currentScale;
            switch (_spriteDefaultFacingDirection)
            {
                case FacingDirection.Right: facingLeftScale.x *= -1; break;
                case FacingDirection.Left: facingRightScale.x *= -1; break;
            }
        }

        public void OnNotifyStart(PlayerController playerController) { }

        public void OnNotifyUpdate(PlayerController playerController)
        {
            // Updates sprite in X axis scale, in order to flip the sprite to the current facing direction
            if (PlayerController.CurrentFacingDirection == FacingDirection.Left) 
                transform.localScale = facingLeftScale;
            else if (PlayerController.CurrentFacingDirection == FacingDirection.Right) 
                transform.localScale = facingRightScale;
        }

    }
}
