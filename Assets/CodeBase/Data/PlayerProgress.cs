using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public Stats HeroStats;
        public KillData KillData;


        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            HeroStats = new Stats();
            KillData = new KillData();
        }
    }
}