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

        public LoadMenuState( SceneLoader sceneLoader, LoadingCurtain loadingCurtain,
            IUiFactory uiFactory)
        {
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(Menu, OnLoaded);
        }

        public void Exit() { }

        private void OnLoaded()
        {
            _loadingCurtain.Hide();
        }
    }
}