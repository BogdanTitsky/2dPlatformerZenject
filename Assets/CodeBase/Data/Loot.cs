using System;

namespace CodeBase.Data
{
    [Serializable]
    public class Loot
    {
        public int Value;
        public Vector3Data PositionOnLevel;
        public string Level;
        public string Id;

        public void SetLootLocation(Vector3Data positionOnLevel, string levelName, string id)
        {
            PositionOnLevel = positionOnLevel;
            Level = levelName;
            Id = id;
        }
    }
}