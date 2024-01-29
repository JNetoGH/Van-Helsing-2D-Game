using PlayerScripts.Enums;
using UnityEngine;

public class CrossbowArmController : MonoBehaviour
{
    
    // Updated by the arms handler 
    // - Required to be out of the arm controller because they're deactivated whe switched
    // - If implemented here the cooldown will get stuck until the Arm is activate again.
    public float ShotCooldownTimer { get; internal set; }
    
    // Same as above but I added a bit of encapsulation as well
    [SerializeField] private float _shootingCooldownDuration = 0.3f;
    public float ShootingCooldownDuration => _shootingCooldownDuration;
    
    // Projectile (Arrow)
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _shootingPoint;
    
    [Header("Sound Settings")]
    [SerializeField] private AudioClip _shootAudioClip;
    private AudioSource _shootAudioSource;
    
    // Others
    private PlayerController _playerController;
    
    private void Start()
    {
        _playerController = GetComponentInParent<PlayerController>();
        
        // sound
        _shootAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        bool isShooting = Input.GetButton("Shoot") || Input.GetButtonDown("Shoot") && !_playerController.IsDashing;
        // cant shoot while dashing
        if(isShooting)
            TryShoot();
    }
    
    private void TryShoot()
    {
        // Method's gateway validations
        if (!(ShotCooldownTimer <= 0)) 
            return;
        
        if (!_playerController.canMove)
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
        
        // Sound
        _shootAudioSource.PlayOneShot(_shootAudioClip);
        
        // Sets the cooldown to start counting
        ShotCooldownTimer = _shootingCooldownDuration;
    }
    
}
