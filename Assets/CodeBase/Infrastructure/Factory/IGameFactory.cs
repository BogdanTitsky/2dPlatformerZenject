using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData.Data;
using CodeBase.Player;
using UnityEngine;
using Projectile = CodeBase.Enemy.RangeAttackLogic.Projectile;

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
        HeroDeath HeroDeathObject { get; }
        void Cleanup();
        Task<LootCollector> CreateLoot();
        Projectile CreateProjectile();
    }
}