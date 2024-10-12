using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData.Data;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        void CreateHero(Vector3 at);
        void CreateCheckPoints(GameObject[] atPoints);
        void CreateHud();
        void CreateCamera();
        void CreateSpawner(Vector3 at, string spawnerId, EnemyTypeId enemyTypeId);
        GameObject CreateEnemy(EnemyTypeId typeId, Transform transformParent);
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        GameObject HeroGameObject { get; }
        void Cleanup();
        LootCollector CreateLoot();
    }
}