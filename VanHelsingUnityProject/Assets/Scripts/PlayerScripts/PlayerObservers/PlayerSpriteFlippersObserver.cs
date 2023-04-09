using Interfaces;
using UnityEngine;

public class PlayerSpriteFlippersObserver: MonoBehaviour, IPlayerObserver
{

    private Vector3 facingRightScale;
    private Vector3 facingLeftScale;
    
    public void OnNotifyStart(PlayerController playerController)
    {
        // scale for sprite sync
        Vector3 currentScale = transform.localScale;
        facingRightScale = currentScale;
        facingLeftScale = currentScale;
        switch (playerController.spriteDefaultFacingDirection)
        {
            case FacingDirection.Right: facingLeftScale.x *= -1; break;
            case FacingDirection.Left: facingRightScale.x *= -1; break;
        }
    }

    public void OnNotifyUpdate(PlayerController playerController)
    {
        // Updates sprite in X axis scale, in order to flip the sprite to the current facing direction
        if (PlayerController.CurrentFacingDirection == FacingDirection.Left) 
            transform.localScale = facingLeftScale;
        else if (PlayerController.CurrentFacingDirection == FacingDirection.Right) 
            transform.localScale = facingRightScale;
    }

}
