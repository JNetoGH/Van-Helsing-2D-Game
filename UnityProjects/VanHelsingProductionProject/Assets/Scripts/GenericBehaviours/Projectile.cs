using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private int _damage = 1; 
    [SerializeField] private bool _disappearWhenEnemyHit = false;
    [SerializeField] private bool _showHitEffect = false;
    [SerializeField] private GameObject _hitEffect;
    private Rigidbody2D _rigidbody2D;
    
    private void Start() => _rigidbody2D = GetComponent<Rigidbody2D>();
  
    private void FixedUpdate() => _rigidbody2D.velocity = transform.right * _moveSpeed;
    
    public void InflictDamageToEnemy(Enemy receiver)
    {
        receiver.DecreaseLife(_damage);

        if (_showHitEffect && _hitEffect is not null)
            Instantiate(_hitEffect, this.transform.position, this.transform.rotation);
        
        if (_disappearWhenEnemyHit)
            Destroy(this.gameObject);
    }
}
