using PlayerScripts.Enums;
using UnityEngine;

public class CrossbowArmController : MonoBehaviour
{
    
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private float _shootingCooldownDuration = 0.3f;
    private float _shotCoolDownTimer = 0;
    public float ShootingCooldownDuration => _shootingCooldownDuration;
    public float ShotCoolDownTimer => _shotCoolDownTimer;
    
    private PlayerController _playerController;
    
    private void Start()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        _shotCoolDownTimer -= Time.deltaTime;
        // needs to be zero in order to sync with the GUI slider
        if (_shotCoolDownTimer < 0)
            _shotCoolDownTimer = 0;
        
        bool isShooting = Input.GetButton("Shoot") || Input.GetButtonDown("Shoot") && !_playerController.IsDashing;
        // cant shoot while dashing
        if(isShooting)
            TryShoot();
    }
    
    private void TryShoot()
    {
        if (!(_shotCoolDownTimer <= 0)) 
            return;
        // arrow is by default facing right --> just like the crossbow
        // I make an alternative rotation in case Van Helsing is facing left because the X scale is * -1
        // and it messes a lot with the arrow inverting the rotation wile facing left, so I subtract 180 of it
        Quaternion projectileRotation = transform.rotation;
        if (_playerController.CurrentFacingDirection == FacingDirection.Left)
        {
            Vector3 aux = projectileRotation.eulerAngles;
            aux.z -= 180;
            projectileRotation = Quaternion.Euler(aux);
        }
        Instantiate(_projectile, _shootingPoint.position, projectileRotation);
        _shotCoolDownTimer = _shootingCooldownDuration;
    }
    
    public void ResetCoolDown() => _shotCoolDownTimer  = 0;
    
}
