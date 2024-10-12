
using System;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Logic;
using CodeBase.UI.Services.Factory;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        public event Action OnLoaded; 

        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IStaticDataService _staticData;
        private readonly IUiFactory _uiFactory;

        public LoadLevelState(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IStaticDataService staticData, IUiFactory uiFactory)
        {
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _staticData = staticData;
            _uiFactory = uiFactory;
        }

        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(sceneName, () => LevelLoaded());
        }

        private void LevelLoaded()
        {
            OnLoaded?.Invoke();
            InitUiRoot();
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private void InitUiRoot() => 
            _uiFactory.CreateUiRoot();
    }
}