using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    
    public void InflictDamageToReceiver(DamageReciever receiver)
    {
        receiver.DecreaseLife(_damage);
    }
}
