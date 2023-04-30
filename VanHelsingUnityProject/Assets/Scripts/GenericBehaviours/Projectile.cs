using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    private Rigidbody2D _rigidbody2D;
    
    private void Start() => _rigidbody2D = GetComponent<Rigidbody2D>();
    private void FixedUpdate() => _rigidbody2D.velocity = transform.right * _moveSpeed;
}
