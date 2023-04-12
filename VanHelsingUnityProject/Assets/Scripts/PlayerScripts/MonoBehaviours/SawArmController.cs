using UnityEngine;

public class SawArmController : MonoBehaviour
{
    
    [SerializeField] private float atkInterval = 0.5f;
    public static  float AtkCoolDownTimer = 0;
    private static readonly int Shoot = Animator.StringToHash("melee");
    private Animator _sawArmAnimator;
    
    private void Start()
    {
        _sawArmAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        AtkCoolDownTimer -= Time.deltaTime;
        if(PlayerController.HasShotThisFrame && !PlayerController.IsDashing) 
            TryAtk();
    }
    
    private void TryAtk()
    {
        if (AtkCoolDownTimer <= 0)
        {
            _sawArmAnimator.SetTrigger(Shoot);
            AtkCoolDownTimer = atkInterval;
        }
    }
    
}
