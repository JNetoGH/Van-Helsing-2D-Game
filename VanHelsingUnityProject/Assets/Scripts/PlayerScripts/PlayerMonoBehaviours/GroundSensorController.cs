using UnityEngine;


public class GroundSensorController : MonoBehaviour
{

    private bool isGorunded = false;
    private float m_DisableTimer;

    private void OnEnable() => isGorunded = false;

    public bool State()
    {
        if (m_DisableTimer > 0)
            return false;
        return isGorunded;
    }

    void OnTriggerEnter2D(Collider2D other) => isGorunded = true;

    void OnTriggerExit2D(Collider2D other) => isGorunded = false;
    
    void Update() => m_DisableTimer -= Time.deltaTime;
    
    public void Disable(float duration) => m_DisableTimer = duration;
    
}