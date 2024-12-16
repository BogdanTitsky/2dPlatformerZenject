using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Random = System.Random;

namespace CodeBase.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private UniqueId IdComponent;
        [SerializeField] private LootType LootType;
        private IGameFactory _factory;
        private int _lootMax;
        private int _lootMin;
        private readonly Random _random = new();

        [Inject]
        public void Init(IGameFactory gameFactory, IPersistentProgressService progressService)
        {
            _factory = gameFactory;
        }

        public void SetLootValue(int min, int max)
        {
            _lootMax = max;
            _lootMin = min;
        }

        public async void SpawnLoot()
        {
          LootCollector lootCollector = await _factory.CreateLoot();
          lootCollector.transform.position = transform.position;
          
          Loot lootItem = GenerateLoot();
          
          lootCollector.InitLootItem(lootItem);
        }

        private Loot GenerateLoot()
        {
            return new Loot(
                _random.Next(_lootMin, _lootMax),
                transform.position.AsVectorData(),
                SceneManager.GetActiveScene().name,
                IdComponent.Id,
                LootType
            );
        }
    }
}