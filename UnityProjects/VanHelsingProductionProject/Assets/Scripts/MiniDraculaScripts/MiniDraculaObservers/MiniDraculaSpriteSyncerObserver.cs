using MiniDraculaScripts.Interfaces;
using MiniDraculaScripts.MiniDraculaEnums;
using UnityEngine;
using UnityEngine.Serialization;

public class MiniDraculaSpriteSyncerObserver : MonoBehaviour, IMiniDraculaObserver
{
    
    [SerializeField] private GameObject _animationFrontView;
    [SerializeField] private GameObject _animationSideView;
    private GameObject _currentAnimationGameObject = null;
    private Vector3 _defaultScale;
    
    public void OnNotifyStart(MiniDraculaController miniDraculaController) 
    {
        _defaultScale = transform.localScale;
        SwitchAnimation(MiniDraculaView.Front);
    }

    public void OnNotifyUpdate(MiniDraculaController miniDraculaController)
    {
        if (miniDraculaController.HasStartedToAtkPlayerAtThisFrame)
        {
            // switches and flips the sprite
            SwitchAnimation(MiniDraculaView.Side);
            FlipSpriteToFaceATarget(miniDraculaController.Player.transform.position);
        }
    }
    
    public void SwitchAnimation(MiniDraculaView miniDraculaView)
    {
        if (miniDraculaView == MiniDraculaView.Front)
            _currentAnimationGameObject = _animationFrontView;
        else if (miniDraculaView == MiniDraculaView.Side)
            _currentAnimationGameObject = _animationSideView;
        foreach (SpriteRenderer spriteRenderer in _animationSideView.GetComponentsInChildren<SpriteRenderer>())
            spriteRenderer.enabled = false;
        foreach (SpriteRenderer spriteRenderer in _animationFrontView.GetComponentsInChildren<SpriteRenderer>())
            spriteRenderer.enabled = false;
        foreach (SpriteRenderer spriteRenderer in _currentAnimationGameObject.GetComponentsInChildren<SpriteRenderer>())
            spriteRenderer.enabled = true;
    }
    
    public void FlipSpriteToFaceATarget(Vector3 targetPosition)
    {
        // flips only at side view
        if (_currentAnimationGameObject != _animationSideView)
            return;
        
        // flips the object by X scale
        if (targetPosition.x > transform.position.x)
            transform.localScale = new Vector3(_defaultScale.x * -1, _defaultScale.y, _defaultScale.z);
        else
            transform.localScale = new Vector3(_defaultScale.x, _defaultScale.y, _defaultScale.z);
    }

}
