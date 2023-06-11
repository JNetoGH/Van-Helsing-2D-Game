using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CooldownMsgController : MonoBehaviour
{
    
    [SerializeField] private GameObject _cooldownMsgPrefab;
    [SerializeField] private float _lifeTime = 1;
    
    [Header("Ascending Animation")]
    [SerializeField] private float _ascendingSpeed;
    
    [Header("Disappearing Animation")]
    [SerializeField] private float _disappearingSpeed;
    private TextMeshProUGUI _textMesh;
    
    private float _timer = 0;
    
    // Others
    private RectTransform _rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // Timer Update
        _timer += Time.deltaTime;
        if (_timer >= _lifeTime)
            Destroy(this.gameObject);
        AscendAnimation();
        DisappearingAnimation();
    }

    private void AscendAnimation()
    {
        // Sf = So + V * T -------->
        Vector3 curPos = _rectTransform.position;
        Vector3 newPos = new Vector3(curPos.x, curPos.y + _ascendingSpeed * Time.deltaTime, curPos.z);
        _rectTransform.position = newPos;
    }

    private void DisappearingAnimation()
    {
        var currColor = _textMesh.color;
        currColor.a -= _disappearingSpeed * Time.deltaTime;
        _textMesh.color = currColor;
    }

    public void InstantiateCooldownMsg(Vector3 worldPos)
    {
        var msg = Instantiate(_cooldownMsgPrefab, GameObject.Find("Canvas").transform);
        Vector3 instantiationPos = Camera.main.WorldToScreenPoint(worldPos);
        instantiationPos.z = 0;
        msg.GetComponent<RectTransform>().position = instantiationPos;
    }
    
}
