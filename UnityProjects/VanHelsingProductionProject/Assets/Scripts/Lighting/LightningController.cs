using UnityEngine;

public class LightningController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerDeathManager.NotifyCurrentManagerAboutPlayerDeath();
        }
    }
}
