using CodeBase.Logic;

namespace CodeBase.Infrastructure.States
{
    public class LoadMenuState : IState
    {
        private const string Menu = "Menu";

        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;

        public LoadMenuState(SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
        {
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter()
        {
            _loadingCurtain.Show();
            _sceneLoader.Load(Menu, OnLoaded);
        }

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            _loadingCurtain.Hide();
        }
    }
}