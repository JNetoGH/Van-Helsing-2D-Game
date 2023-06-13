using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraculaHPBarController : MonoBehaviour
{

    public Enemy Dracula { get; set; }
    [SerializeField] private Slider _bar;

    // Update is called once per frame
    void Update()
    {
        if (Dracula is null)
        {
            Debug.LogWarning($"Dracula is null");
            return;
        }

        Debug.LogWarning($"Dracula Max HP: {Dracula.maxHealthPoints}  Dracula HP: {Dracula.HealthPoints}");
        _bar.maxValue = Dracula.maxHealthPoints;
        _bar.value = Dracula.HealthPoints;
    }
}
