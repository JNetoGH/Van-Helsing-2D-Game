using UnityEngine;

public class PlayerDeathManager : MonoBehaviour
{

    [SerializeField] private AscendAndDisappearText _ascendAndDisappearText;
    private static AscendAndDisappearText _ascendAndDisappearTextStaticWrapper;
    
    // set by the floors themselves
    public static IFloorManager currentFloorManager;
    public static bool isPlayerInvincible = false;

    private void Start()
    {
        _ascendAndDisappearTextStaticWrapper = _ascendAndDisappearText;
    }

    public static void NotifyCurrentManagerAboutPlayerDeath()
    {
        if (isPlayerInvincible)
            return;
        currentFloorManager.OnPlayerDead();
        _ascendAndDisappearTextStaticWrapper.InstantiateMsgAtScreenMiddle();
    }
    
}
