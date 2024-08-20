using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private DiContainer _container;
        private readonly IStaticDataService _staticData;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();

        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();

        public GameObject HeroGameObject { get; set; }

        public GameFactory(DiContainer container, IAssetProvider assets, IStaticDataService staticData)
        {
            _container = container;
            _assets = assets;
            _staticData = staticData;
        }

        public void CreateHero(GameObject at)
        {
            HeroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);
        }

        public GameObject CreateEnemy(EnemyTypeId typeId, Transform parent)
        {
           EnemyStaticData enemyData = _staticData.ForEnemy(typeId);
           
           GameObject enemy = _container.InstantiatePrefab(enemyData.Prefab);
           enemy.transform.position = parent.position;
           
           IHealth health = enemy.GetComponent<IHealth>();
           health.Current = enemyData.Hp;
           health.Max = enemyData.Hp;
           
           EnemyAttack attack = enemy.GetComponent<EnemyAttack>();
           attack.Damage = enemyData.Damage;
           attack.Cleavage = enemyData.Cleavage;
           attack.AttackCooldown = enemyData.AttackCooldown;
           attack.Distance = enemyData.Distance;

           var lootSpawner = enemy.GetComponentInChildren<LootSpawner>();
           lootSpawner.SetLootValue(enemyData.MinLoot, enemyData.MaxLoot);
           
           return enemy;
        }
        
        public LootCollector CreateLoot()
        {
            var coin = InstantiateRegistered(AssetPath.Coin);
            return coin.GetComponent<LootCollector>();
        }
        
        public void CreateCheckPoints(GameObject[] atPoints)
        {
            foreach (var checkPoint in atPoints)
                InstantiateRegistered(AssetPath.CheckPointPath, checkPoint.transform.position);
        }

        public void CreateCamera() =>
            InstantiateRegistered(AssetPath.CameraPath);

        public void CreateHud() =>
            InstantiateRegistered(AssetPath.HudPath);

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            GameObject gameObject = _container.InstantiatePrefabResource(prefabPath);
            gameObject.transform.position = at;
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _container.InstantiatePrefabResource(prefabPath);

            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }

        public void Register(ISavedProgressReader progressReader)
        {
            if(progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);
      
            ProgressReaders.Add(progressReader);
        }

        
    }
}