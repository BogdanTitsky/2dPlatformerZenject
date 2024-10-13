using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Infrastructure.Services.StaticData.Data;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawner;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private const string SaveTriggerTag = "SaveTriggerPoint";
        private readonly IGameStateMachine _stateMachine;
        private readonly IPersistentProgressService _progressService;
        private DiContainer _container;
        private readonly IStaticDataService _staticData;
        private LoadLevelState _loadLevelState;
        private IAssetProvider _assets;
        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        public GameObject HeroGameObject { get; set; }

        public GameFactory(IGameStateMachine stateMachine,DiContainer sceneContainer, IStaticDataService staticData, LoadLevelState loadLevelState, IPersistentProgressService progressService, IAssetProvider assetProvider)
        {
            _progressService = progressService;
            _assets = assetProvider;
            _stateMachine = stateMachine;
            _container = sceneContainer;
            _staticData = staticData;
            _loadLevelState = loadLevelState;
            _loadLevelState.OnLoaded += LoadGame;

        }

        private async void LoadGame()
        {
            await InitGameWorld();
            await TryCreateUncollectedLoot();
            InformProgressReaders();
            _stateMachine.Enter<GameLoopState>();
            _loadLevelState.OnLoaded -= LoadGame;

        }
        
        private async Task InitGameWorld()
        {
            Cleanup();
            LevelStaticData levelData = GetLevelStaticData();
            await InitSpawners(levelData);
            await CreateHero(levelData.InitialHeroPosition);
            await CreateHud();
            await CreateCheckPoints(GameObject.FindGameObjectsWithTag(SaveTriggerTag));
        }
        
        private void InformProgressReaders()
        {
            foreach (var progressReader in ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }

        private async Task TryCreateUncollectedLoot()
        {
            foreach (var lootItem in _progressService.Progress.WorldData.NotCollectedLoot.NotCollectedList)
            {
                LootCollector lootObject = await CreateLoot();
                lootObject.InitLootItem(lootItem);
                lootObject.transform.position = lootItem.PositionOnLevel.AsUnityVector();
            }
        }
        
        private async Task InitSpawners(LevelStaticData levelData)
        {
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawnerData) 
                await CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.EnemyTypeId);
        }
        
        private LevelStaticData GetLevelStaticData() => 
            _staticData.ForLevel(SceneManager.GetActiveScene().name);

        public async Task CreateHero(Vector3 at)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Hero);

            HeroGameObject = InstantiateRegistered(prefab, at);
        }

        public async Task<GameObject> CreateEnemy(EnemyTypeId typeId, Transform parent)
        {
           EnemyStaticData enemyData = _staticData.ForEnemy(typeId);

           GameObject prefab = await _assets.Load<GameObject>(enemyData.prefabReference);
           
           GameObject enemy = _container.InstantiatePrefab(prefab);
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

        public async Task<LootCollector> CreateLoot()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.LootCoin);

            return InstantiateRegistered(prefab).GetComponent<LootCollector>();
        }

        public async Task CreateCheckPoints(GameObject[] atPoints)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.CheckPoint);

            foreach (var checkPoint in atPoints)
                InstantiateRegistered(prefab, checkPoint.transform.position);
        }

        public async Task CreateCamera()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Camera);
            InstantiateRegistered(prefab);
        }

        public async Task CreateHud()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Hud);
            InstantiateRegistered(prefab);
        }

        public async Task CreateSpawner(Vector3 at, string spawnerId, EnemyTypeId enemyTypeId)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);

            var spawner = InstantiateRegistered(prefab, at)
                .GetComponent<EnemySpawnPoint>();

            spawner.Id = spawnerId;
            spawner.EnemyTypeId = enemyTypeId;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
            
            _assets.Cleanup();
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = _container.InstantiatePrefab(prefab);
            gameObject.transform.position = at;
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = _container.InstantiatePrefab(prefab);

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