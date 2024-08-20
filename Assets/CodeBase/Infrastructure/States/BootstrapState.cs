using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private GameStateMachine _stateMachine;
        private SceneLoader _sceneLoader;
        private ISaveLoadService _saveLoadService;
        private  IStaticDataService _staticDataService;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, IStaticDataService staticDataService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            _staticDataService.LoadEnemies();
            _sceneLoader.Load(Initial, EnterLoadLevel);
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