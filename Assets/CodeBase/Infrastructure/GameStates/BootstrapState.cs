using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Infrastructure.States;

namespace CodeBase.Infrastructure.GameStates
{
    public class BootstrapGameState : IGameState
    {
        private IGameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;
        private ISaveLoadService _saveLoadService;
        private IStaticDataService _staticDataService;
        private IAssetProvider _assets;

        public BootstrapGameState(IGameStateMachine stateMachine, SceneLoader sceneLoader, IStaticDataService staticDataService, IAssetProvider assets)
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
            _stateMachine.Enter<LoadProgressGameState>();
        }

        public void Exit()
        {
        }
    }
}