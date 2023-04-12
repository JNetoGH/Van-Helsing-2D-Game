using PlayerScripts.Enums;
using UnityEngine;


public class CrossbowShootingHandler : MonoBehaviour
{
    
    [SerializeField] private GameObject projectile;
    private Transform shotPoint;
    private float _shotInterval = 0.3f;
    private float _shotCoolDownTimer = 0;
    
    //public Animator camAnim;


    private void Start()
    {
        shotPoint = GetComponentInChildren<Transform>();
    }

    private void Update()
    {
        if(PlayerController.IsShooting) 
            TryShoot();
    }
    
    private void TryShoot()
    {
        if (Input.GetButton("Shoot") && _shotCoolDownTimer <= 0)
        {
            // not implemented animation
            // camAnim.SetTrigger("shake");
            
            // arrow is by default facing right --> just like the crossbow
            // I make an alternative rotation in case Van Helsing is facing left because the X scale is * -1
            // and it messes a lot with the arrow inverting the rotation wile facing left, so I subtract 180 of it
            Quaternion projectileRotation = transform.rotation;
            if (PlayerController.CurrentFacingDirection == FacingDirection.Left)
            {
                Vector3 aux = projectileRotation.eulerAngles;
                aux.z -= 180;
                projectileRotation = Quaternion.Euler(aux);
            }
            Instantiate(projectile, shotPoint.position, projectileRotation);
            _shotCoolDownTimer = _shotInterval;
            return;
        }
        _shotCoolDownTimer -= Time.deltaTime;
    }
}
