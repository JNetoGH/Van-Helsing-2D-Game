using UnityEngine;

public class ArrowController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 10f;

    private Rigidbody2D _rigidbody2D;

    private void Start() => _rigidbody2D = GetComponent<Rigidbody2D>();

    private void FixedUpdate() => _rigidbody2D.velocity = transform.right * moveSpeed;
    
}