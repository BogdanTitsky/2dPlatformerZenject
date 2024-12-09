
using System;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Logic;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class ReloadLevelState : IPayloadedState<string>
    {
        public event Action ReloadLevel;
        private IGameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private IAssetProvider _assets;
        

        public ReloadLevelState(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameStateMachine stateMachine)
        {
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _stateMachine = stateMachine;
        }
        public void Enter(string payload)
        {
            _loadingCurtain.Show();
            ReloadLevel?.Invoke();
            _loadingCurtain.Hide();
            _stateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
        }
    }
}