using PlayerScripts.Enums;
using UnityEngine;

public class CrossbowArmController : MonoBehaviour
{
    
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private float shotInterval = 0.3f;
    public static float ShotCoolDownTimer = 0;
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        if(_playerController.IsShooting) 
            TryShoot();
    }
    
    private void TryShoot()
    {
        if (ShotCoolDownTimer <= 0)
        {
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
            Instantiate(projectile, shotPoint.position, projectileRotation);
            ShotCoolDownTimer = shotInterval;
            return;
        }
        ShotCoolDownTimer -= Time.deltaTime;
    }
    
}
