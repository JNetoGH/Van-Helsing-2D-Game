using UnityEngine;

public class DustDestroyer : MonoBehaviour
{
    // Called by the animation when finished
    public void DestroyDust()
    {
        Destroy(this.gameObject);
    }
}
