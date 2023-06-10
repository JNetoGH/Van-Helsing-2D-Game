using UnityEngine;
using UnityEngine.Serialization;

public class DamageDealer : MonoBehaviour
{
    
    [SerializeField] private int _damage = 1; 
    [SerializeField] private bool _disappearWhenEnemyHit = false;
    
    public void InflictDamageToReceiver(DamageReciever receiver)
    {
        receiver.DecreaseLife(_damage);
        if (_disappearWhenEnemyHit)
            Destroy(this.gameObject);
    }
}
