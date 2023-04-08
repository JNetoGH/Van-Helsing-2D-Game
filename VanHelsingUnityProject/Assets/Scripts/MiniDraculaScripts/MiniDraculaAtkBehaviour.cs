using UnityEngine;

public class MiniDraculaAtkBehaviour : MonoBehaviour
{
    
    private const float CountDownTimerToAttackPlayerDurationInSecs = 3;
    // private const float AtkDurationInSec = 2;
    private float _attackCounter;
    private GameObject _player;
    private Vector2 _directionToPlayer = new Vector2(0, 0);
    private float _flySpeed = 2;
    private MiniDraculaSpriteAndAniController _spriteControllerScript;
    
    public static bool IsInAttackingPlayerState { get; private set; } = false;
    public static bool HasStartedToAtkPlayerAtThisFrame { get; private set; } = false;

    void Start()
    {
        _attackCounter = CountDownTimerToAttackPlayerDurationInSecs;
        _spriteControllerScript = GetComponent<MiniDraculaSpriteAndAniController>();
        _player = GameObject.FindWithTag("Player");
        if (_player is not null)
            Debug.Log("mini Dracula found the player");
    }
    
    void Update()
    {
        if (_attackCounter <= 0)
        {
            IsInAttackingPlayerState = true;
            HasStartedToAtkPlayerAtThisFrame = true;
            _attackCounter = CountDownTimerToAttackPlayerDurationInSecs;
        }

        if (!IsInAttackingPlayerState)
        {
            _attackCounter -= Time.deltaTime;
            return;
        }
  
        // Attack Start-Up
        if (HasStartedToAtkPlayerAtThisFrame)
        {
            // switches and flips the sprite
            _spriteControllerScript.SwitchAnimation(MiniDraculaView.Side);
            _spriteControllerScript.FlipSpriteToFaceATarget(_player.transform.position);
            // generates the direction to player
            _directionToPlayer = (_player.transform.position - this.transform.position).normalized;
            // Locks the Attack Start-Up to do not happen again
            HasStartedToAtkPlayerAtThisFrame = false;
            Debug.Log("mini Dracula will atk player now");
        }
    
        // Atk
        Vector3 incrementOnPos = _directionToPlayer * (_flySpeed * Time.deltaTime);
        incrementOnPos.z = 0;
        transform.position = transform.position + incrementOnPos;
    }
    
    private void PrintDebugInfo()
    {
        Debug.Log($"IsInAttackingPlayerState {IsInAttackingPlayerState}");
        Debug.Log($"HasStartedToAtkPlayerAtThisFrame {HasStartedToAtkPlayerAtThisFrame}");
    }
    
}
