using System;
using UnityEngine;

public class SawArmController : MonoBehaviour
{
    
    [SerializeField] private float atkInterval = 0.5f;
    private float _atkCoolDownTimer = 0;
    private static readonly int Shoot = Animator.StringToHash("shoot");
    
    private Animator _sawArmAnimator;
    
    private void Start()
    {
        _sawArmAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        _atkCoolDownTimer -= Time.deltaTime;
        if(PlayerController.HasShotThisFrame && !PlayerController.IsDashing) 
            TryAtk();
    }
    
    private void TryAtk()
    {
        if (_atkCoolDownTimer <= 0)
        {
            Debug.Log("Shoot");
            _sawArmAnimator.SetTrigger(Shoot);
            _atkCoolDownTimer = atkInterval;
        }
    }
    
}
