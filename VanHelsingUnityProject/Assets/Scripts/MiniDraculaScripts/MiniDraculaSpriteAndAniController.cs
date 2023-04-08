using UnityEngine;

public enum MiniDraculaView
{
    Front,
    Side
}

public class MiniDraculaSpriteAndAniController : MonoBehaviour
{
    
    [SerializeField] private GameObject animationFrontView;
    [SerializeField] private GameObject animationSideView;
    [SerializeField] private MiniDraculaView defaultAnimationMiniDraculaView;
    private GameObject _currentAnimationGameObject = null;
    private Vector3 _defaultScale;
    
    void Start()
    {
        _defaultScale = transform.localScale;
        SwitchAnimation(defaultAnimationMiniDraculaView);
    }
    
    public void SwitchAnimation(MiniDraculaView miniDraculaView)
    {
        if (miniDraculaView == MiniDraculaView.Front)
            _currentAnimationGameObject = animationFrontView;
        else if (miniDraculaView == MiniDraculaView.Side)
            _currentAnimationGameObject = animationSideView;
        
        foreach (SpriteRenderer spriteRenderer in animationSideView.GetComponentsInChildren<SpriteRenderer>())
            spriteRenderer.enabled = false;
        foreach (SpriteRenderer spriteRenderer in animationFrontView.GetComponentsInChildren<SpriteRenderer>())
            spriteRenderer.enabled = false;
        foreach (SpriteRenderer spriteRenderer in _currentAnimationGameObject.GetComponentsInChildren<SpriteRenderer>())
            spriteRenderer.enabled = true;
    }
    
    public void FlipSpriteToFaceATarget(Vector3 targetPosition)
    {
        // flips only at side view
        if (_currentAnimationGameObject != animationSideView)
            return;
        
        // flips the object by X scale
        if (targetPosition.x > transform.position.x)
            transform.localScale = new Vector3(_defaultScale.x * -1, _defaultScale.y, _defaultScale.z);
        else
            transform.localScale = new Vector3(_defaultScale.x, _defaultScale.y, _defaultScale.z);
    }
    
}
