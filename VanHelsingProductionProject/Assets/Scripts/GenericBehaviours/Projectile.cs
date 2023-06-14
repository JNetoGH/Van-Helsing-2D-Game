using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private int _damage = 1; 
    [SerializeField] private bool _disappearWhenEnemyHit = false;
    [SerializeField] private bool _showHitEffect = false;
    [SerializeField] private GameObject _hitEffect;

    public bool setDirManually = false;
    public Vector3 direction = Vector3.zero;
    private Rigidbody2D _rigidbody2D;

    private bool _hasHitDracula = false;
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (setDirManually)
            _rigidbody2D.velocity = direction * _moveSpeed;
        else
            _rigidbody2D.velocity = transform.right * _moveSpeed;
    }

    public void InflictDamageToEnemy(Enemy receiver)
    {
        if (receiver.tag.Equals("Dracula"))
        {
            if (!_hasHitDracula)
            {
                receiver.DecreaseLife(_damage);
                _hasHitDracula = true;
            }
        }
        else
        {
            receiver.DecreaseLife(_damage);
        }

        if (_showHitEffect && _hitEffect is not null)
            Instantiate(_hitEffect, this.transform.position, this.transform.rotation);
        
        if (_disappearWhenEnemyHit)
            Destroy(this.gameObject);
        
    }
}
