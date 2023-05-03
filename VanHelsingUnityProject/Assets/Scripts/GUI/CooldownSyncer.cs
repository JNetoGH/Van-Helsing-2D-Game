using UnityEngine;
using UnityEngine.UI;

public class CooldownSyncer : MonoBehaviour
{
   
    [Header("Cooldown Sliders")]
    [SerializeField] private Slider _crossbowCooldownSlider;
    [SerializeField] private Slider _sawCooldownSlider;
    [SerializeField] private Slider _dashCooldownSlider;
    
    [Header("Scripts With The Cooldown Timers")]
    [SerializeField] private CrossbowArmController _crossbowArmController;
    [SerializeField] private SawArmController _sawArmController;
    [SerializeField] private PlayerController _playerController;

    private void Start()
    {
        _crossbowCooldownSlider.maxValue = _crossbowArmController.ShootingCooldownDuration;
        _sawCooldownSlider.maxValue = _sawArmController.AttackCooldownDuration;
        _dashCooldownSlider.maxValue = _playerController.DashCooldownInSec;
    }

    // Update is called once per frame
    void Update()
    {
        _crossbowCooldownSlider.value = _crossbowArmController.ShotCoolDownTimer;
        _sawCooldownSlider.value = _sawArmController.AtkCoolDownTimer;
        _dashCooldownSlider.value = _playerController.DashCooldownTimer;
    }
    
}
