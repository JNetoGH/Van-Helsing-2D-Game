using PlayerScripts.Enums;
using UnityEngine;
using UnityEngine.Serialization;

public class SawArmController : MonoBehaviour
{
    
    // Updated by the arms handler 
    // - Required to be out of the arm controller because they're deactivated whe switched
    // - If implemented here the cooldown will get stuck until the Arm is activate again.
    public float AtkCooldownTimer { get; internal set; } = 0;
   
    // Same as above but I added a bit of encapsulation as well
    [SerializeField] private float _attackCooldownDuration = 0.5f;
    public float AttackCooldownDuration => _attackCooldownDuration;
    
    [Header("Area of Effect DMG")]
    [SerializeField] private GameObject _sawAreaOfEffect;
    [SerializeField] private Transform _areaOfEffectInstantiationPoint;
    
    [Header("Saw Cooldown Msg")]
    [SerializeField] private CooldownMsgController _cooldownMsgController;
    [SerializeField] private Transform _cooldownMsgInstantiationWorldPos;
    
    // Others
    private PlayerController _playerController;
    private Animator _sawArmAnimator;
    private static readonly int ShootAnimatorParameter = Animator.StringToHash("melee");

    private void Start()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _sawArmAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        // cant shoot while dashing
        bool hasShot = Input.GetButtonDown("Shoot") && !_playerController.IsDashing;
        if(hasShot) 
            TryAtk();
    }
    
    private void TryAtk()
    {
        // Method's gateway validations
        if (!(AtkCooldownTimer <= 0))
        {
            var msgPos = _cooldownMsgInstantiationWorldPos.transform.position;
            _cooldownMsgController.InstantiateCooldownMsg(msgPos);
            return;
        }
        
        if (!_playerController.canMove)
            return;

        // Animator
        _sawArmAnimator.SetTrigger(ShootAnimatorParameter);
        
        // AoE Instantiation
        GameObject AoE = Instantiate(_sawAreaOfEffect);
        
        // AoE positioning
        AoE.transform.position = _areaOfEffectInstantiationPoint.transform.position;
       
        // AoE rotation
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;
        Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        AoE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        // AoE sprite flip 
        if (_playerController.CurrentFacingDirection == FacingDirection.Left)
            AoE.GetComponent<SpriteRenderer>().flipY = true;

        // Arm Cooldown Reset
        AtkCooldownTimer = _attackCooldownDuration;
    }
    
}
