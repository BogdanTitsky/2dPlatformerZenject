using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Infrastructure.Services.StaticData.Data;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawner;
using CodeBase.UI.Elements;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : BaseFactory, IGameFactory, IDisposable
    {
        public override List<ISavedProgressReader> ProgressReaders { get; } = new();
        public override List<ISavedProgress> ProgressWriters { get; } = new();
        public HeroDeath HeroDeathObject { get; private set; }
        private const string SaveTriggerTag = "SaveTriggerPoint";
        private readonly IGameStateMachine _stateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private LoadLevelState _loadLevelState;
        private IAssetProvider _assets;
        private DiContainer _container;

        private LootDisplay _lootDisplay;

        public GameFactory(DiContainer container, IGameStateMachine stateMachine, IStaticDataService staticData, LoadLevelState loadLevelState, IPersistentProgressService progressService, IAssetProvider assets) : base(container, assets, progressService)
        {
            _progressService = progressService;
            _assets = assets;
            _stateMachine = stateMachine;
            _staticData = staticData;
            _loadLevelState = loadLevelState;
            _container = container;
            _loadLevelState.OnLoaded += LoadGame;
        }

        private async void LoadGame()
        {
            await InitGameWorld();
            await TryCreateUncollectedLoot();
            InformProgressReaders();
            _stateMachine.Enter<GameLoopState>();
        }
        public void Dispose() => 
            _loadLevelState.OnLoaded -= LoadGame;

        private async Task InitGameWorld()
        {
            Cleanup();
            LevelStaticData levelData = GetLevelStaticData();
            await InitSpawners(levelData);
            await CreateHero(levelData.InitialHeroPosition);
            await CreateHud();
            await CreateCheckPoints(GameObject.FindGameObjectsWithTag(SaveTriggerTag));
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

            HeroDeathObject = InstantiateRegistered(prefab, at).GetComponent<HeroDeath>();
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
           
           EnemyMeleeAttackBehaviour meleeAttackBehaviour = enemy.GetComponent<EnemyMeleeAttackBehaviour>();
           IEnemyStateMachine stateMachine = new EnemyStateMachine();
           
           meleeAttackBehaviour.Damage = enemyData.Damage;
           meleeAttackBehaviour.AttackCooldown = enemyData.AttackCooldown;
           meleeAttackBehaviour.StateMachine = stateMachine;

           LootSpawner lootSpawner = enemy.GetComponentInChildren<LootSpawner>();
           lootSpawner.SetLootValue(enemyData.MinLoot, enemyData.MaxLoot);
           
           return enemy;
        }

        public async Task<LootCollector> CreateLoot()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.LootCoin);
            LootCollector lootCollector = InstantiateRegistered(prefab).GetComponent<LootCollector>();

            return lootCollector;
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
            if (Application.platform == RuntimePlatform.WindowsPlayer ||
                Application.platform == RuntimePlatform.OSXPlayer ||
                Application.platform == RuntimePlatform.LinuxPlayer)
            {
                return;
            }
            
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Hud);
            GameObject instance = InstantiateRegistered(prefab);
            _lootDisplay = instance.GetComponentInChildren<LootDisplay>();
            _container.Bind<LootDisplay>().FromInstance(_lootDisplay).AsSingle();
        }

        public async Task CreateSpawner(Vector3 at, string spawnerId, EnemyTypeId enemyTypeId)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);

            EnemySpawnPoint spawner = InstantiateRegistered(prefab, at)
                .GetComponent<EnemySpawnPoint>();

            spawner.Id = spawnerId;
            spawner.EnemyTypeId = enemyTypeId;
        }

        
    }
}