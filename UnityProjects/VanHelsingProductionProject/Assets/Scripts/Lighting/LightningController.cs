using UnityEngine;

public class LightningController : MonoBehaviour
{
    
    public ThirdFloorManager thirdFloorManager;


    private void Update()
    {
        if (thirdFloorManager is null)
            Debug.LogWarning("Third Floor Manager Script is missing");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            thirdFloorManager.OnPlayerDead();
        }
    }
}
