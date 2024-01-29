using System;
using System.IO;
using UnityEngine;

namespace DataPersistenceSystem
{
    public class GameDataSerializationHandler
    {
        
        // Path.Combine is better than concatenating strings, cuz different OS can have different separators like the / 
        private string FullPath => Path.Combine(_dataDirectoryPath, _dataFileName);
        private string _dataDirectoryPath = "";
        private string _dataFileName = "";

        public GameDataSerializationHandler(string dataDirectoryPath, string dataFileName)
        {
            _dataDirectoryPath = dataDirectoryPath;
            _dataFileName = dataFileName;

            // in case the file doesn't exist, makes a new one
            if (!File.Exists(FullPath))
            {
                FileStream creation = new FileStream(FullPath, FileMode.CreateNew);
                creation.Close();
            }
        }
        
        /// <returns> The Data class or null if doesn't manage to Deserialize </returns>
        public GameSerializableData DeserializeFromFile()
        {
            GameSerializableData loadedData = null;
            try
            {
                // Reads the Json: FileMode.Open Specifies that the operating system should open an existing file
                string dataAsJsonToLoad = "";
                using FileStream stream = new FileStream(FullPath, FileMode.Open);
                using StreamReader reader = new StreamReader(stream);
                dataAsJsonToLoad = reader.ReadToEnd();

                // Deserializes the data
                loadedData = JsonUtility.FromJson<GameSerializableData>(dataAsJsonToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"Attempt to load the game data at {FullPath} failed, {e}");
            }
            return loadedData;
        }

        public void SerializeDataToFile(GameSerializableData gameSerializableData)
        {
            try
            {
                // in case the directory doesn't exist yet, CreateDirectory(_dataDirectoryPath) should also work.
                Directory.CreateDirectory(Path.GetDirectoryName(FullPath)); 
                
                // Serializes the Game Date into a .json file
                string dataToBeStoredAsJson = JsonUtility.ToJson(gameSerializableData, true);
                
                // Writes the Json file:
                // FileMode.Create, specifies that the operating system should create a new file.
                // If the file already exists, it will be overwritten.
                using FileStream stream = new FileStream(FullPath, FileMode.Create);
                using StreamWriter writer = new StreamWriter(stream);
                writer.Write(dataToBeStoredAsJson);
            }
            catch (Exception e)
            {
                Debug.LogError($"Attempt to save the game data at {FullPath} failed, {e}");
            }
        }
        
    }
}