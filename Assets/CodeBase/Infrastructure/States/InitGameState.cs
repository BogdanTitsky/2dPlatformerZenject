using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class InitGameState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IAudioFactory _audioFactory;
        private readonly IAssetProvider _assets;
        private readonly IUiFactory _uiFactory;

        public InitGameState(IGameStateMachine gameStateMachine, IAudioFactory audioFactory, 
            IAssetProvider assets, IUiFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            
            _audioFactory = audioFactory;
            _assets = assets;
            _uiFactory = uiFactory;
        }

        public async void Enter()
        {
            _audioFactory.SetupAudio();
            await _uiFactory.CreateUiRoot();
            _uiFactory.InitWindows();
            _gameStateMachine.Enter<LoadMenuState>();
        }

        public void Exit()
        {
        }
    }
}