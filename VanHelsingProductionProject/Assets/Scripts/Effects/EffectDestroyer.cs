using UnityEngine;

public class EffectDestroyer : MonoBehaviour
{
    // Called by the animation when finished
    public void DestroyEffect()
    {
        Destroy(this.gameObject);
    }
}
