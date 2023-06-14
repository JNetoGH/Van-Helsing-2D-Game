using System;
using DataPersistenceSystem;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    
    // Singleton Pattern 
    public DataPersistenceManager Instance { get; private set; }
    private GameSerializableData _gameSerializableData;
    
    private void Awake()
    {
        if (Instance is not null)
        {
            Debug.LogError("There is already a Data Persistence Manager on scene, " +
                           "its a singleton and there should only be one per scene");
        }
        Instance = this;
    }

    private void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        // Warning msg
        if (_gameSerializableData is null)
            Debug.LogWarning("Tried to load game data but there is none");
        
        // Loading data from file
        
        // Pushing data to the scriptable objets
    }

    public void SaveData()
    {
        // Warning msg
        if (_gameSerializableData is null)
            Debug.LogWarning("Tried to save game data but there is none");
        
        // Passes the data to the file
        
        // Finishes by Reloading the data with the changes
        LoadData();
    }
    
}
