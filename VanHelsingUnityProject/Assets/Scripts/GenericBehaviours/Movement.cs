using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum KindOfMovement 
{
    Continuous,
    Loop,
    Once
}

public class Movement : MonoBehaviour 
{
    
    [SerializeField] private float durationInSec = 3.2f;
    [SerializeField] private float speed = 2;
    [SerializeField] private KindOfMovement kindOfMovement;
    [SerializeField] private Vector2 moveDirection = new Vector2(1, 1);

    private float _moveTimer = 0;

    private void Start() 
    {
        if (moveDirection.magnitude != 0)
            moveDirection = moveDirection.normalized;
    }

    private void Move()
    {
        if (durationInSec <= _moveTimer && kindOfMovement != KindOfMovement.Continuous)
        {
            if (kindOfMovement != KindOfMovement.Loop) 
                return;
            _moveTimer = 0;
            moveDirection.x *= -1;
            moveDirection.y *= -1;
        }
        _moveTimer += Time.deltaTime;
        transform.Translate(moveDirection * (speed * Time.deltaTime));
    }
    
    void Update() => Move();
    
}
