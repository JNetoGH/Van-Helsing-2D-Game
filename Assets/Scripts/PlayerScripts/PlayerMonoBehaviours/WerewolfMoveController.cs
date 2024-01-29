using PlayerScripts.Enums;
using UnityEngine;

public class WerewolfMoveController : MonoBehaviour
{
    
    [SerializeField] GameObject _crossbowArm;
    [SerializeField] GameObject _crossbow;
    [SerializeField] GameObject _sawArm;
    [SerializeField] GameObject _saw;
    private bool _hideArms;
    
    [Header("Projectile Settings")] 
    [SerializeField] private GameObject _werewolfProjectilePrefab;
    [SerializeField] private Transform _werewolfProjectileInstantiationPoint;
    
    // Called by an animation event at the start
    public void HideArms()
    {
        _hideArms = true;
        
        GameObject projectile = Instantiate(_werewolfProjectilePrefab);
        projectile.transform.position = _werewolfProjectileInstantiationPoint.position;

        PlayerController playerController = GetComponent<PlayerController>();
        Vector3 projDir = playerController.CurrentFacingDirection == FacingDirection.Right
            ? Vector3.right
            : Vector3.left;

        if (projDir == Vector3.left)
            projectile.GetComponent<SpriteRenderer>().flipX = true;

        projectile.GetComponent<Projectile>().setDirManually = true;
        projectile.GetComponent<Projectile>().direction = projDir;
    }

    // Called by an animation event at the end
    public void ShowArmsWrapper()
    {
        // the animation has a delay, so i am fixing it here in a quick way
        Invoke(nameof(ShowArms), 0.2f);
    }

    private void ShowArms()
    {
        _hideArms = false;
    }

    // Update is called once per frame
    void Update()
    {
        _crossbowArm.GetComponent<SpriteRenderer>().enabled = !_hideArms;
        _sawArm.GetComponent<SpriteRenderer>().enabled = !_hideArms;
        _saw.GetComponent<SpriteRenderer>().enabled = !_hideArms;
        _crossbow.GetComponent<SpriteRenderer>().enabled = !_hideArms;
    }
    
}
