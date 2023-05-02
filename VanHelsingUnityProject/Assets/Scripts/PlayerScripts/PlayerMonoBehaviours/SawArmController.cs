using UnityEngine;

public class SawArmController : MonoBehaviour
{
    
    [SerializeField] private float _attackCoolDownDuration = 0.5f;
    private float _atkCoolDownTimer = 0;
    private PlayerController _playerController;
    private Animator _sawArmAnimator;
    private static readonly int Shoot = Animator.StringToHash("melee");

    private void Start()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _sawArmAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        _atkCoolDownTimer -= Time.deltaTime;
        // cant shoot while dashing
        bool hasShot = Input.GetButton("Shoot") && !_playerController.IsDashing;
        if(hasShot) 
            TryAtk();
    }
    
    private void TryAtk()
    {
        if (!(_atkCoolDownTimer <= 0)) return;
        _sawArmAnimator.SetTrigger(Shoot);
        _atkCoolDownTimer = _attackCoolDownDuration;
    }

    public void ResetCoolDown() => _atkCoolDownTimer = 0;
    
}
