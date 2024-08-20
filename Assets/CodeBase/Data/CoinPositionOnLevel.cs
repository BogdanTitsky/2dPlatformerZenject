using System;

namespace CodeBase.Data
{
    [Serializable]
    public class CoinPositionOnLevel
    {
        public string Level;
        public Vector3Data Position;

        public CoinPositionOnLevel(string level)
        {
            Level = level;
        }

        public void SetPosition(Vector3Data position)
        {
            Position = position;
        }
    }
}