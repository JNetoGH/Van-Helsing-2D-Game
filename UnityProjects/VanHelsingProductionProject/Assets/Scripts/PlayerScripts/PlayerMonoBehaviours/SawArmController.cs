using PlayerScripts.Enums;
using UnityEngine;

public class SawArmController : MonoBehaviour
{
    
    [SerializeField] private float _attackCooldownDuration = 0.5f;
    [SerializeField] private GameObject _sawAreaOfEffect;
    [SerializeField] private Transform _areaOfEffectInstantiationPoint;
    private float _atkCoolDownTimer = 0;
    public float AttackCooldownDuration => _attackCooldownDuration;
    public float AtkCoolDownTimer => _atkCoolDownTimer;
    
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
        _atkCoolDownTimer -= Time.deltaTime;
        // needs to be zero in order to sync with the GUI slider
        if (_atkCoolDownTimer < 0)
            _atkCoolDownTimer = 0;
        
        // cant shoot while dashing
        bool hasShot = Input.GetButtonDown("Shoot") && !_playerController.IsDashing;
        if(hasShot) 
            TryAtk();
    }
    
    private void TryAtk()
    {
        if (!(_atkCoolDownTimer <= 0)) return;
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
        
        // sets the sprite flip to match
        if (_playerController.CurrentFacingDirection == FacingDirection.Left)
            AoE.GetComponent<SpriteRenderer>().flipY = true;
        
        // Cooldown Reset
        _atkCoolDownTimer = _attackCooldownDuration;
    }

    public void ResetCoolDown() => _atkCoolDownTimer = 0;
    
}
