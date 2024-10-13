using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private IGameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;
        private ISaveLoadService _saveLoadService;
        private IStaticDataService _staticDataService;
        private IAssetProvider _assets;

        public BootstrapState(IGameStateMachine stateMachine, SceneLoader sceneLoader, IStaticDataService staticDataService, IAssetProvider assets)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _staticDataService = staticDataService;
            _assets = assets;
        }

        public void Enter()
        {
            _staticDataService.LoadStaticData();
            _assets.Initialize();
            _sceneLoader.Load("Menu", EnterLoadLevel);
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }
    }
}