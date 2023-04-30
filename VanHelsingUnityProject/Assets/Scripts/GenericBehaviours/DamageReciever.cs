using UnityEngine;

public class DamageReciever : MonoBehaviour
{
    
    [Header("HP System")]
    [SerializeField] private int _healthPoints = 1;
    
    [Header("Damage Color Effect")]
    [SerializeField] private float _dmgColorIndicatorDurationInSec = 0.1f;
    [SerializeField] private SpriteRenderer[] _spriteRenderersToReceiveDmgEffect;
    
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
        _healthPoints -= damage;
        if (_healthPoints < 0)
            _healthPoints = 0;
    }

    private void ExecuteDmgEffect()
    {
        foreach (SpriteRenderer spriteRenderer in _spriteRenderersToReceiveDmgEffect)
            spriteRenderer.color = Color.red;
        Invoke(nameof(ClearSprites), _dmgColorIndicatorDurationInSec);
    }

    private void ClearSprites()
    {
        foreach (SpriteRenderer spriteRenderer in _spriteRenderersToReceiveDmgEffect)
            spriteRenderer.color = Color.white;
    }
    
}
