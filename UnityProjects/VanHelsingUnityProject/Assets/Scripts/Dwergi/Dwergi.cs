using System;
using Unity.Mathematics;
using UnityEngine;

namespace Dwergi
{
    public class Dwergi : MonoBehaviour
    {
        [SerializeField] private float     moveSpeed = 100.0f;
        [SerializeField] private float detectionRange;
        private bool _hasDetectedPlayer = false;

        private Rigidbody2D _rigidbody;
        private Transform _transform;
        private Animator _animator;
        private Vector2 _movementDirection;
        private Vector2 _newVelocity;
        private bool _isWalking = false;
        private GameObject Player { get; set; }
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _transform = GetComponent<Transform>();
            _movementDirection = new Vector2(0, 0);
            Player = GameObject.FindWithTag("Player");
        
            if (Player is null)
            {
                Debug.LogWarning("_player not found");
                return;
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            TryDetectingPlayer();
            if (_hasDetectedPlayer) GetPlayerDirection();
            else _movementDirection.x = 0;
            
            _newVelocity.x = _movementDirection.x * (moveSpeed * Time.deltaTime);
            _newVelocity.y = _rigidbody.velocity.y;
            _rigidbody.velocity = _newVelocity;

            if (_rigidbody.velocity.x > 0.1 || _rigidbody.velocity.x < -0.1)
            {
                _isWalking = true;
            }
            else _isWalking = false;
            
            _animator.SetFloat("isWalking", Convert.ToSingle(_isWalking));
        
            if (_rigidbody.velocity.x < 0)
            {
                if (transform.right.x > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
            else if (_rigidbody.velocity.x > 0)
            {
                if (transform.right.x < 0)
                {
                    transform.rotation = Quaternion.identity;
                }
            }
        }

        private void GetPlayerDirection()
        {
            _movementDirection.x = Player.transform.position.x - _transform.position.x;
            _movementDirection.Normalize();
        }

        private void TryDetectingPlayer()
        {
            Vector2 dwergiTransform = _transform.position;
            Vector2 newPlayerTransform = Player.transform.position;

            if (Vector2.Distance(dwergiTransform, newPlayerTransform) <= detectionRange)
            {
                
                _hasDetectedPlayer = true;
            }
            else
            {
                _hasDetectedPlayer = false;
            }
            
        }
    }
}
