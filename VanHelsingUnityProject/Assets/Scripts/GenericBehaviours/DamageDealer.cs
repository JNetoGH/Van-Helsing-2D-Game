using UnityEngine;


public class DamageDealer : MonoBehaviour
{
    public int damage = 1;
    public void InflictDamageToReceiver(DamageEffect receiver) 
    {
        receiver.DecreaseLife(damage);
    }
}
