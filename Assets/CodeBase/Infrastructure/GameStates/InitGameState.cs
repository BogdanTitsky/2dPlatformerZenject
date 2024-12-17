using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.UI.Services.Factory;
using UnityEditor;

namespace CodeBase.Infrastructure.GameStates
{
    public class InitGameGameState : IGameState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IAudioFactory _audioFactory;
        private readonly IUiFactory _uiFactory;
        private const string LaunchScene = "LaunchScene";
        private const string Menu = "Menu";
        private const string DefaultSceneName = "Initial";

        public InitGameGameState(IGameStateMachine gameStateMachine, IAudioFactory audioFactory, IUiFactory uiFactory)
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
            string sceneName;

#if UNITY_EDITOR
            sceneName = EditorPrefs.GetString(LaunchScene,
                DefaultSceneName);
#else
            sceneName = DefaultSceneName;
#endif

            if (sceneName is Menu or DefaultSceneName)
                _stateMachine.Enter<LoadMenuGameState>();
            else
                _stateMachine.Enter<LoadLevelState, string>(sceneName);
        }

        public void Exit()
        {
        }
    }
}