using UnityEngine;

public enum KindOfMovement 
{
    Continuous,
    Loop,
    Once
}

public class Movement : MonoBehaviour 
{
    
    [SerializeField] private float _durationInSec = 3.2f;
    [SerializeField] private float _speed = 2;
    [SerializeField] private KindOfMovement _kindOfMovement = KindOfMovement.Continuous;
    [SerializeField] private Vector2 _moveDirection = new Vector2(1, 0);

    private float _moveTimer = 0;

    private void Start() 
    {
        if (_moveDirection.magnitude != 0)
            _moveDirection = _moveDirection.normalized;
    }

    private void Move()
    {
        if (_durationInSec <= _moveTimer && _kindOfMovement != KindOfMovement.Continuous)
        {
            if (_kindOfMovement != KindOfMovement.Loop) 
                return;
            _moveTimer = 0;
            _moveDirection.x *= -1;
            _moveDirection.y *= -1;
        }
        _moveTimer += Time.deltaTime;
        transform.Translate(_moveDirection * (_speed * Time.deltaTime));
    }
    
    void Update() => Move();    
    
}
