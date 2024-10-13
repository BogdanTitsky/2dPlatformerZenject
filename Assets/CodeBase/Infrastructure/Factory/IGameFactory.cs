using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData.Data;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        Task CreateHero(Vector3 at);
        Task CreateCheckPoints(GameObject[] atPoints);
        Task CreateHud();
        Task CreateCamera();
        Task CreateSpawner(Vector3 at, string spawnerId, EnemyTypeId enemyTypeId);
        Task<GameObject> CreateEnemy(EnemyTypeId typeId, Transform transformParent);
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        GameObject HeroGameObject { get; }
        void Cleanup();
        Task<LootCollector> CreateLoot();
    }
}