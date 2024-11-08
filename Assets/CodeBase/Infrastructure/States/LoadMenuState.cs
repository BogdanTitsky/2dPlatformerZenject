using CodeBase.Audio;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadMenuState : IState
    {
        private const string Menu = "Menu";

        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IUiFactory _uiFactory;
        private readonly IGameStateMachine _gameStateMachine;
        private IAssetProvider _assets;
        private IAudioFactory _audioFactory;

        public LoadMenuState(SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            IUiFactory uiFactory, IGameStateMachine gameStateMachine, IAssetProvider assets, IAudioFactory audioFactory)
        {
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _uiFactory = uiFactory;
            _gameStateMachine = gameStateMachine;
            _assets = assets;
            _audioFactory = audioFactory;
        }

        public async void Enter()
        {
            _loadingCurtain.Show();
            await _assets.Load<GameObject>(AssetAddress.UIRoot);
            _sceneLoader.Load(Menu, OnLoaded);
        }

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            InitUiRoot();
            _audioFactory.SetupAudio();
            _loadingCurtain.Hide();
        }
        
        private void InitUiRoot() => 
            _uiFactory.CreateUiRoot();
    }
}