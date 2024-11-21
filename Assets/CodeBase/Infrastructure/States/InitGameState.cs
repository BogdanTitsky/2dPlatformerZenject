using CodeBase.Infrastructure.Factory;
using CodeBase.UI.Services.Factory;
using UnityEditor;

namespace CodeBase.Infrastructure.States
{
    public class InitGameState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IAudioFactory _audioFactory;
        private readonly IUiFactory _uiFactory;
        private const string LaunchScene = "LaunchScene";
        private const string Menu = "Menu";
        private const string DefaultSceneName = "Initial";

        public InitGameState(IGameStateMachine gameStateMachine, IAudioFactory audioFactory, IUiFactory uiFactory)
        {
            _stateMachine = gameStateMachine;
            _audioFactory = audioFactory;
            _uiFactory = uiFactory;
        }

        public async void Enter()
        {
            _audioFactory.SetupAudio();
            await _uiFactory.CreateUiRoot();
            _uiFactory.InitWindows();

            //Tools/Scene Launch Settings
            string sceneName = EditorPrefs.GetString(LaunchScene, DefaultSceneName);

            if (sceneName == Menu)
                _stateMachine.Enter<LoadMenuState>();
            else
                _stateMachine.Enter<LoadLevelState, string>(sceneName);
        }

        public void Exit()
        {
        }
    }
}