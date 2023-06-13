using UnityEngine;

public class Movement : MonoBehaviour 
{

    private enum TypeOfMovement
    {
        Continuous,
        Once
    }
    
    public enum TargetPoint
    {
        PointA,
        PointB
    }
    
    [Header("Waiting Timer Settings")]
    [SerializeField] private bool _waitBeforeStart = false;
    [SerializeField] private float _waitingDuration;
    private float _waitingTimer;
    
    [Header("Movement Setting")]
    [SerializeField] private TypeOfMovement _typeOfMovement;
    [SerializeField] private TargetPoint _targetPointWrapper = TargetPoint.PointB;
    [SerializeField] private float _speed = 3.2f;
    [SerializeField] private Vector3 _pointA;
    [SerializeField] private Vector3 _pointB;
    private Vector3 _targetPoint;

    public void TeleportToPoint(TargetPoint target)
    {
        switch (target)
        {
            case TargetPoint.PointA:
                transform.position = _pointA;
                _targetPointWrapper = TargetPoint.PointB;
                break;
            case TargetPoint.PointB: 
                transform.position = _pointB;
                _targetPointWrapper = TargetPoint.PointA;
                break;
        }
    }

    public void ResetWaitingTimer()
    {
        _waitingTimer = _waitingDuration;
    }
    
    private void Start()
    {
        transform.position = _targetPointWrapper == TargetPoint.PointA ? _pointB : _pointA;
        _waitingTimer = _waitingDuration;
    }

    private void Update()
    {

        // Waiting Timer
        if (_waitBeforeStart)
        {
            _waitingTimer -= Time.deltaTime;
            if (_waitingTimer > 0)
                return;
        }
        
        // Movement Itself
        _targetPoint = _targetPointWrapper == TargetPoint.PointA ? _pointA : _pointB;
        transform.position = Vector3.MoveTowards(
            transform.position, 
            _targetPoint,
            _speed * Time.deltaTime);
        
        // Continuous Movement Checker
        if (_typeOfMovement != TypeOfMovement.Continuous)
            return;
            
        // Switches the current target
        if (transform.position == _targetPoint)
            _targetPointWrapper = _targetPointWrapper == TargetPoint.PointA ? TargetPoint.PointB : TargetPoint.PointA;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(_pointA, 0.1f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(_pointB, 0.1f);
    }
}
