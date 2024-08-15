using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        void CreateHero(GameObject at);
        void CreateCheckPoints(GameObject[] atPoints);
        void CreateHud();
        void CreateCamera();
        GameObject CreateEnemy(EnemyTypeId typeId, Transform transformParent);
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        GameObject HeroGameObject { get; }

        void Cleanup();
        void Register(ISavedProgressReader savedProgress);
    }
}