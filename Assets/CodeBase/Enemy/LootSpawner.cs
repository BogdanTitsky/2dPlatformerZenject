using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace CodeBase.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyDeath EnemyDeath;
        private IGameFactory _factory;
        private int _lootMax;
        private int _lootMin;
        private readonly Random _random = new();
        private WorldData _worldData;

        [Inject]
        public void Init(IGameFactory gameFactory, IPersistentProgressService progressService)
        {
            _factory = gameFactory;
            _worldData = progressService.Progress.WorldData;
        }

        private void OnEnable() => 
            EnemyDeath.OnDeath += SpawnLoot;

        private void OnDisable() => 
            EnemyDeath.OnDeath -= SpawnLoot;

        public void SetLootValue(int min, int max)
        {
            _lootMax = max;
            _lootMin = min;
        }

        private void SpawnLoot()
        {
          LootCollector loot = _factory.CreateLoot();
          loot.transform.position = transform.position;
          
          Loot lootItem = GenerateLoot();
          
          loot.InitLootItem(lootItem);
          _worldData.CoinPositionOnLevel.SetPosition(loot.transform.position.AsVectorData());
        }

        private Loot GenerateLoot()
        {
            return new()
            {
                Value = _random.Next(_lootMin, _lootMax)
            };
        }

        
    }
}