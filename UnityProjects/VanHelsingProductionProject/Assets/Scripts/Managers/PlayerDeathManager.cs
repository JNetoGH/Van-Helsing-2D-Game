using UnityEngine;

public class PlayerDeathManager : MonoBehaviour
{
    
    // set by the floors themselves
    public static IFloorManager currentFloorManager;
    
    public static void NotifyCurrentManagerAboutPlayerDeath()
    {
        currentFloorManager.OnPlayerDead();
    }
    
}
