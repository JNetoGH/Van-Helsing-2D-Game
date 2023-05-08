using UnityEngine;

public class SawArmController : MonoBehaviour
{
    
    [SerializeField] private float _attackCooldownDuration = 0.5f;
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
        _atkCoolDownTimer = _attackCooldownDuration;
    }

    public void ResetCoolDown() => _atkCoolDownTimer = 0;
    
}
