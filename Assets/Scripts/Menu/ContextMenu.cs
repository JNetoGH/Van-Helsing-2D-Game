using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenu : MonoBehaviour
{
    
    [SerializeField] private List<GameObject> _mainMenuObjects;
    
    public void ToggleToMainMenu()
    {
        _mainMenuObjects.ForEach(e => e.SetActive(true));
        this.gameObject.SetActive(false);
    }

    public void ToggleToThisContextMenu ()
    {
        this.gameObject.SetActive(true);
        _mainMenuObjects.ForEach(e => e.SetActive(false));
    }
    
}
