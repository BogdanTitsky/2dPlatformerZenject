using System;

namespace CodeBase.Data
{
  [Serializable]
  public class PositionOnLevel
  {
    public string SceneName;
    public Vector3Data Position;

    public PositionOnLevel(string sceneName, Vector3Data position)
    {
      SceneName = sceneName;
      Position = position;
    }

    public PositionOnLevel(string initialSceneName)
    {
      SceneName = initialSceneName;
    }
  }
}