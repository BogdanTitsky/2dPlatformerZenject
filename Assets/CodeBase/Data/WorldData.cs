using System;

namespace CodeBase.Data
{
  [Serializable]
  public class WorldData
  {
    public PositionOnLevel PositionOnLevel;
    public LootData LootData;
    public NotCollectedLoot NotCollectedLoot;


    public WorldData(string initialLevel)
    {
      PositionOnLevel = new PositionOnLevel(initialLevel);
      LootData = new LootData();
      NotCollectedLoot = new NotCollectedLoot();
    }
  }
}