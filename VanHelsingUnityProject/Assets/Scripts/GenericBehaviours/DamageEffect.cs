using UnityEngine;

public class DamageEffect : MonoBehaviour
{
 
    [SerializeField] private int healthPoints = 1;
    [SerializeField] private float dmgColorIndicatorDurationInSec = 0.1f;
    [SerializeField] private SpriteRenderer[] spriteRenderersToReceiveDmgEffect;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        // checks if its trigger has collided with a game object that carries a DamageDealer
        if (col.gameObject.GetComponent<DamageDealer>() is null)
            return;
        col.gameObject.GetComponent<DamageDealer>().InflictDamageToReceiver(this);
        ExecuteDmgEffect();
    }
    
    public void DecreaseLife(int damage) 
    {
        healthPoints -= damage;
        if (healthPoints < 0)
            healthPoints = 0;
    }

    private void ExecuteDmgEffect()
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderersToReceiveDmgEffect)
            spriteRenderer.color = Color.red;
        Invoke(nameof(ClearSprites), dmgColorIndicatorDurationInSec);
    }

    private void ClearSprites()
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderersToReceiveDmgEffect)
            spriteRenderer.color = Color.white;
    }
    
}
