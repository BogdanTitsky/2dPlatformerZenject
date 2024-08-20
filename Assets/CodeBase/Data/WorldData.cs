using System;

namespace CodeBase.Data
{
  [Serializable]
  public class WorldData
  {
    public PositionOnLevel PositionOnLevel;
    public CoinPositionOnLevel CoinPositionOnLevel;
    public LootData LootData;


    public WorldData(string initialLevel)
    {
      PositionOnLevel = new PositionOnLevel(initialLevel);
      CoinPositionOnLevel = new CoinPositionOnLevel(initialLevel);
      LootData = new LootData();
    }
  }
}