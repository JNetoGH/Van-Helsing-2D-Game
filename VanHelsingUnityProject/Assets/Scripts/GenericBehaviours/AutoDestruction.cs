using UnityEngine;

public class AutoDestruction : MonoBehaviour
{
    
    [SerializeField] private float LifeTimeInSeconds = 4;
    
    private void Start() => Invoke(nameof(DestroyItself), LifeTimeInSeconds);
    
    private void DestroyItself() => Destroy(this.transform.gameObject);

    private void OnDestroy() => Debug.Log($"{this.name} got auto-destructed");
    
}
