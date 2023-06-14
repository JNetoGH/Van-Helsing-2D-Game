namespace DataPersistenceSystem
{
    public interface IDataPersistenceListener
    {
        void OnLoadData(GameSerializableData gameSerializableData);
        void OnSaveData(GameSerializableData gameSerializableData);
    }
}