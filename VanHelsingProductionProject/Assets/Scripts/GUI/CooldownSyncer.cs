using UnityEngine;
using UnityEngine.UI;

public class CooldownSyncer : MonoBehaviour
{
   
    [Header("Cooldown Sliders")]
    [SerializeField] private Slider _crossbowCooldownSlider;
    [SerializeField] private Slider _sawCooldownSlider;
    [SerializeField] private Slider _dashCooldownSlider;
    [SerializeField] private Slider _werewolfCooldownSlider;
    
    [Header("Scripts With The Cooldown Timers")]
    [SerializeField] private CrossbowArmController _crossbowArmController;
    [SerializeField] private SawArmController _sawArmController;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private FourthFloorManager _fourthFloorManager;

    private void Start()
    {
        _crossbowCooldownSlider.maxValue = _crossbowArmController.ShootingCooldownDuration;
        _sawCooldownSlider.maxValue = _sawArmController.AttackCooldownDuration;
        _dashCooldownSlider.maxValue = _playerController.DashCooldownInSec;
        if (_werewolfCooldownSlider is not null && _fourthFloorManager is not null)
            _werewolfCooldownSlider.maxValue = FourthFloorManager.WerewolfCooldownDurationInSec;
    }

    // Update is called once per frame
    void Update()
    {
        _crossbowCooldownSlider.value = _crossbowArmController.ShotCooldownTimer;
        _sawCooldownSlider.value = _sawArmController.AtkCooldownTimer;
        _dashCooldownSlider.value = _playerController.DashCooldownTimer;
        if (_werewolfCooldownSlider is not null && _fourthFloorManager is not null)
            _werewolfCooldownSlider.value = _fourthFloorManager.WerewolfCooldownTimer;
    }
    
}
