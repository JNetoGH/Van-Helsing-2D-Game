using System.Collections.Generic;
using System.Linq;
using DataPersistenceSystem;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    
    // Singleton Pattern 
    public static DataPersistenceManager Instance { get; private set; }

    [Header("File Storage Settings")] [SerializeField]
    private string fileName;
    
    public GameSerializableData GameSerializableData  { get; private set; }
    private GameDataSerializationHandler _gameDataSerializationHandler;
    private List<IDataPersistenceListener> _listeners;
    
    private void Awake()
    {
        if (Instance is not null)
            Debug.LogWarning(
                "There is already a Data Persistence Manager set to manager's Instance, " +
                "it's a singleton and there should only be one, this one is overriding the previous one");
        Instance = this;
    }

    private void Start()
    {
        // Application.persistentDataPat: Contains the path to a persistent data directory for that OS's
        // Ex in Windows: C:\Users\<user>\AppData\LocalLow\<company name>
        _gameDataSerializationHandler = new GameDataSerializationHandler(Application.persistentDataPath, fileName);
        _listeners = FindAllListeners();
        
        // Gives a blank Data set to the game 
        GameSerializableData = new GameSerializableData();
        
        // Creates the file in case it doesn't exist or if it exists but doesn't deserializes
        // and gives the default game data to it
        if (_gameDataSerializationHandler.DeserializeFromFile() is null)
            SaveData();
        
        // Loads the data to the game
        LoadData();
    }
    
    public void SaveData()
    {
        // Passes the data to the file
        _gameDataSerializationHandler.SerializeDataToFile(GameSerializableData);
        
        // Updates the listeners in case there is any one new and pushes data to the scriptable listeners
        _listeners = FindAllListeners();
        foreach (IDataPersistenceListener listener in _listeners)
            listener.OnSaveData(gameSerializableData: GameSerializableData);
    }

    public void LoadData()
    {
        // Loading data from file, can return null
        GameSerializableData = _gameDataSerializationHandler.DeserializeFromFile();
        
        // Warning msg
        if (GameSerializableData is null)
        {
            Debug.LogWarning("Tried to load game data but there is none");
            GameSerializableData = new GameSerializableData();
            return;
        }
        
        // Updates the listeners in case there is any one new and pushes data to the scriptable listeners
        _listeners = FindAllListeners();
        foreach (IDataPersistenceListener listener in _listeners)
            listener.OnLoadData(gameSerializableData: GameSerializableData);
    }
    
    private List<IDataPersistenceListener> FindAllListeners()
    {
        IEnumerable<IDataPersistenceListener> listeners =
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistenceListener>();
        return new List<IDataPersistenceListener>(listeners);
    }

}
