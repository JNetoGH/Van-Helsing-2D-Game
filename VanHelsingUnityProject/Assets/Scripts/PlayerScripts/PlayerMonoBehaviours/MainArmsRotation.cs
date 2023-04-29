using PlayerScripts.Enums;
using UnityEngine;


public class MainArmsRotation : MonoBehaviour
{
    
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = transform.parent.GetComponent<PlayerController>();
        transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Making sure that the angle in 0 is zero
    }

    void Update()
    {
        RotateArmAccordingToMousePos();
    }

    private void RotateArmAccordingToMousePos()
    {
        Vector3 mouseInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 distanceToMouse = mouseInWorldPos - transform.position;
        float rotZ = Mathf.Atan2(distanceToMouse.y, distanceToMouse.x) * Mathf.Rad2Deg;
        
        switch (_playerController.CurrentFacingDirection)
        {
            case FacingDirection.Right: transform.rotation = Quaternion.Euler(0f, 0f, rotZ); break;
            case FacingDirection.Left:  transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 180); break;
        }
    }

}
