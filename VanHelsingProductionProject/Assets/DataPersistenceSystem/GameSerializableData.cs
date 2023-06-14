using System;

namespace DataPersistenceSystem
{   
    [Serializable]
    public class GameSerializableData
    {
        public bool invincibility = false;
        public bool ignoreFirstFloorSequence = false;
        public bool ignoreSecondFloorSequence = false;
        public bool ignoreThirdFloorSequence = false;
        public bool ignoreFourthFloorSequence = false;
    }
}