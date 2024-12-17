using CodeBase.Logic;

namespace CodeBase.Infrastructure.GameStates
{
    public class LoadMenuGameState : IGameState
    {
        private const string Menu = "Menu";

        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;

        public LoadMenuGameState(SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
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