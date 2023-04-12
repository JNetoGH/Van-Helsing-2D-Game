using PlayerScripts.Enums;
using UnityEngine;


public class MainArmRotation : MonoBehaviour
{

    // Making sure that the angle in 0 is zero
    private void Start() => transform.rotation = Quaternion.Euler(0f, 0f, 0f);

    void Update() => RotateArmAccordingToMousePos();

    private void RotateArmAccordingToMousePos()
    {
        Vector3 mouseInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = mouseInWorldPos - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        
        switch (PlayerController.CurrentFacingDirection)
        {
            case FacingDirection.Right: transform.rotation = Quaternion.Euler(0f, 0f, rotZ); break;
            case FacingDirection.Left:  transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 180); break;
        }
    }

}
