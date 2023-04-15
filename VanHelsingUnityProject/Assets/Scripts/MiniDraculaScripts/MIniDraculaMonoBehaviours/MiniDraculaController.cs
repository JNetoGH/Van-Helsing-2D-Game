using System.Collections.Generic;
using MiniDraculaScripts.Interfaces;
using UnityEngine;

public class MiniDraculaController : MonoBehaviour
{
    
    private readonly List<IMiniDraculaObserver> _miniDraculaObservers = new List<IMiniDraculaObserver>();
 
    public GameObject Player { get; set; }
    [HideInInspector] public float flySpeed = 2;
    private const float IdleDurationInSecs = 3;
    private float _atkCountDownTimer;
    
    public bool IsInAttackPlayerState { get; private set; } = false;
    public bool HasStartedToAtkPlayerAtThisFrame { get; private set; } = false;

    void Start()
    {
        _miniDraculaObservers.Add(GetComponent<MiniDraculaSpriteSyncerObserver>());
        _atkCountDownTimer = IdleDurationInSecs;
        Player = GameObject.FindWithTag("Player");
        
        if (Player is null)
        {
            Debug.LogWarning("Player not found");
            return;
        }
        
        foreach (IMiniDraculaObserver observer in _miniDraculaObservers)
            observer.OnNotifyStart(this);
    }
    
    void Update()
    {
        
        if (Player is null)
        {
            Debug.LogWarning("Player not found");
            return;
        }
        
        HasStartedToAtkPlayerAtThisFrame = false;
        _atkCountDownTimer -= Time.deltaTime;
        if (_atkCountDownTimer <= 0)
        {
            IsInAttackPlayerState = true;
            HasStartedToAtkPlayerAtThisFrame = true;
            // _atkCountDownTimer = IdleDurationInSecs; RESETS TIMER
        }
        
        if(IsInAttackPlayerState)
            AtkPlayer();
        
        foreach (IMiniDraculaObserver observer in _miniDraculaObservers)
            observer.OnNotifyUpdate(this);
    }

    private void AtkPlayer()
    {
        Vector2 dirToPlayer = (Player.transform.position - transform.position).normalized;
        Vector3 incrementOnPos = dirToPlayer * (flySpeed * Time.deltaTime);
        incrementOnPos.z = 0;
        transform.position += incrementOnPos;
    }

}
