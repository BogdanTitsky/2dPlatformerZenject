using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Infrastructure.Services.StaticData.Data;
using CodeBase.Logic;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string SaveTriggerTag = "SaveTriggerPoint";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticData;
        private readonly  IUiFactory _uiFactory;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            IGameFactory gameFactory, IPersistentProgressService progressService, IStaticDataService staticData, IUiFactory uiFactory)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticData = staticData;
            _uiFactory = uiFactory;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _gameFactory.Cleanup();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private void OnLoaded()
        {
            InitGameWorld();
            InformProgressReaders();
            TryCreateUncollectedLoot();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InitGameWorld()
        {
            LevelStaticData levelData = LevelStaticData();

            InitUiRoot();
            InitSpawners(levelData);
            InitHero(levelData);
            _gameFactory.CreateCamera();
            _gameFactory.CreateHud();
            _gameFactory.CreateCheckPoints(GameObject.FindGameObjectsWithTag(SaveTriggerTag));
        }

        private void InformProgressReaders()
        {
            foreach (var progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }

        private void TryCreateUncollectedLoot()
        {
            foreach (var lootItem in _progressService.Progress.WorldData.NotCollectedLoot.NotCollectedList)
            {
                LootCollector lootObject = _gameFactory.CreateLoot();
                lootObject.InitLootItem(lootItem);
                lootObject.transform.position = lootItem.PositionOnLevel.AsUnityVector();
            }
        }

        private LevelStaticData LevelStaticData() => 
            _staticData.ForLevel(SceneManager.GetActiveScene().name);

        private void InitUiRoot() => 
            _uiFactory.CreateUiRoot();

        private void InitHero(LevelStaticData levelData) => 
            _gameFactory.CreateHero(levelData.InitialHeroPosition);

        private void InitSpawners(LevelStaticData levelData)
        {
            foreach (EnemySpawnerData spawnerData in levelData.EnemySpawnerData) 
                _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.EnemyTypeId);
        }
    }
}