using UnityEngine;

public class AutoDestruction : MonoBehaviour
{
    [SerializeField] private float _lifeTimeInSeconds = 10;
    
    private void Start() => Invoke(nameof(DestroyItself), _lifeTimeInSeconds);
    private void DestroyItself() => Destroy(this.transform.gameObject);
    private void OnDestroy() => Debug.Log($"{this.name} got auto-destructed");
}
