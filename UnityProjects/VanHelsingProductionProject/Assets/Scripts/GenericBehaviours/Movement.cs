using UnityEngine;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour 
{

    private enum TypeOfMovement
    {
        Continuous,
        Once
    }
    
    private enum TargetPoint
    {
        PointA,
        PointB
    }
    
    [SerializeField] private TypeOfMovement _typeOfMovement;
    [SerializeField] private TargetPoint _targetPointWrapper;
    [SerializeField] private float _speed = 3.2f;
    [SerializeField] private Vector3 _pointA;
    [SerializeField] private Vector3 _pointB;
    private Vector3 _targetPoint;
    
    private void Start()
    {
        transform.position = _targetPointWrapper == TargetPoint.PointA ? _pointB : _pointA;
    }

    private void Update()
    {
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
