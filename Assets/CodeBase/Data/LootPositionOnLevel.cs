using System;

namespace CodeBase.Data
{
    [Serializable]
    public class LootPositionOnLevel
    {
        public string Level;
        public Vector3Data Position;

        public LootPositionOnLevel(string level)
        {
            Level = level;
        }

        public void SetPosition(Vector3Data position)
        {
            Position = position;
        }
    }
}