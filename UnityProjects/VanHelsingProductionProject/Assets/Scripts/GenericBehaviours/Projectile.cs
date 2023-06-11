using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private int _damage = 1; 
    [SerializeField] private bool _disappearWhenEnemyHit = false;
    private Rigidbody2D _rigidbody2D;
    
    private void Start() => _rigidbody2D = GetComponent<Rigidbody2D>();
  
    private void FixedUpdate() => _rigidbody2D.velocity = transform.right * _moveSpeed;
    
    public void InflictDamageToEnemy(Enemy receiver)
    {
        receiver.DecreaseLife(_damage);
        if (_disappearWhenEnemyHit)
            Destroy(this.gameObject);
    }
}
