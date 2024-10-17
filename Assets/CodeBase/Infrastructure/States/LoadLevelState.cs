
using System;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Logic;
using CodeBase.UI.Services.Factory;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        public event Action OnLoaded; 

        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IUiFactory _uiFactory;
        private IAssetProvider _assets;


        public LoadLevelState(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IUiFactory uiFactory, IAssetProvider assets)
        {
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _uiFactory = uiFactory;
            _assets = assets;
        }

        public async void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            await WarmUp();
            _sceneLoader.Load(sceneName, SceneLoaded);
        }

        private void SceneLoaded()
        {
            OnLoaded?.Invoke();
            InitUiRoot();
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }
        
        private async Task WarmUp()
        {
            await _assets.Load<GameObject>(AssetAddress.Hero);
            await _assets.Load<GameObject>(AssetAddress.Hud);
            await _assets.Load<GameObject>(AssetAddress.Camera);
            await _assets.Load<GameObject>(AssetAddress.CheckPoint);
            await _assets.Load<GameObject>(AssetAddress.LootCoin);
            await _assets.Load<GameObject>(AssetAddress.Spawner);
            await _assets.Load<GameObject>(AssetAddress.UIRoot);
        }

        private void InitUiRoot() => 
            _uiFactory.CreateUiRoot();
    }
}