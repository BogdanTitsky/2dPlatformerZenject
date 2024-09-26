using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.StaticData.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public string LevelKey;
        public List<EnemySpawnerData> EnemySpawnerData;
        public Vector3 InitialHeroPosition;
    }
}