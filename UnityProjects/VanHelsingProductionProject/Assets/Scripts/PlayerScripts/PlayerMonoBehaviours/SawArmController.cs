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
    
    [Header("Cooldown Msg")]
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _cooldownMsg;
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
        // Method's gateway validation
        if (!(AtkCooldownTimer <= 0))
        {
            InstantiateCooldownMsg();
            return;
        }

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

    private void InstantiateCooldownMsg()
    {
        GameObject msg = Instantiate(_cooldownMsg, _canvas.transform);
        Vector3 instantiationPos = Camera.main.WorldToScreenPoint(_cooldownMsgInstantiationWorldPos.position);
        instantiationPos.z = 0;
        msg.GetComponent<RectTransform>().position = instantiationPos;
    }
    
}
