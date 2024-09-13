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
        public LootType LootType;

        public Loot(int value, Vector3Data positionOnLevel, string levelName, string id, LootType lootType)
        {
            Value = value;
            PositionOnLevel = positionOnLevel;
            Level = levelName;
            Id = id;
            LootType = lootType;
        }
    }

    public enum LootType
    {
        Coin,
        Gem,
    }
}