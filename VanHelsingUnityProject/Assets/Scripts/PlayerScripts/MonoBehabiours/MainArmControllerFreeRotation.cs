using PlayerScripts.Enums;
using UnityEngine;


public class MainArmControllerFreeRotation : MonoBehaviour
{    
    
    [SerializeField] private GameObject crossbow;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shotPoint;
    private float _shotInterval = 0.3f;
    private float _shotCoolDown = 0;
    public Animator camAnim;

    private void Start()
    {
        // Making sure that the angle in 0 is zero
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        RotateArmAccordingToMousePos();
        if(PlayerController.IsShooting)
            TryShoot();
    }

    private void RotateArmAccordingToMousePos()
    {
        Vector3 mouseInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = mouseInWorldPos - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        switch (PlayerController.CurrentFacingDirection)
        {
            case FacingDirection.Right:
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
                break;
            case FacingDirection.Left:
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 180);
                break;
        }
    }

    private void TryShoot()
    {
        if (Input.GetButton("Shoot") && _shotCoolDown <= 0)
        {
            // camAnim.SetTrigger("shake");
            // arrow is by default facing right --> just like the crossbow
            // I make an alternative rotation in case Van Helsing is facing left because the X scale is * -1
            // and it messes a lot with the arrow inverting the rotation wile facing left, so I subtract 180 of it
            Quaternion projectileRotation = crossbow.transform.rotation;
            if (PlayerController.CurrentFacingDirection == FacingDirection.Left)
            {
                Vector3 aux = projectileRotation.eulerAngles;
                aux.z -= 180;
                projectileRotation = Quaternion.Euler(aux);
            }
            Instantiate(projectile, shotPoint.position, projectileRotation);
            _shotCoolDown = _shotInterval;
            return;
        }
        _shotCoolDown -= Time.deltaTime;
    }
    
}

