using System;

namespace DataPersistenceSystem
{   
    [Serializable]
    public class GameSerializableData
    {
        public bool invincibility;
        
        public bool ignoreFirstFloorSequence;
        public bool ignoreSecondFloorSequence;
        public bool ignoreThirdFloorSequence;
        public bool ignoreForthFloorSequence;
        
        public int testNum;
    }
}