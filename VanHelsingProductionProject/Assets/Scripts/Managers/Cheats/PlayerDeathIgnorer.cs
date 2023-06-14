using DataPersistenceSystem;
using UnityEngine;

public class PlayerDeathIgnorer : MonoBehaviour, IDataPersistenceListener
{

    [Header("Ignored (WILL BE OVERRIDEN BY THE DATA PERSISTENCE SYSTEM)")]
    [SerializeField] private bool _isPlayerInvincible;
    
    private void Start()
    {
        PlayerDeathManager.isPlayerInvincible = _isPlayerInvincible;
    }

    private void Update()
    {
        PlayerDeathManager.isPlayerInvincible = _isPlayerInvincible;
    }

    public void OnLoadData(GameSerializableData gameSerializableData)
    {
        _isPlayerInvincible = gameSerializableData.invincibility;
    }

    public void OnSaveData(GameSerializableData gameSerializableData) { }
}
