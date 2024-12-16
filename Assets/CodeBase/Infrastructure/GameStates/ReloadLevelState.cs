using System;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.GameStates;
using CodeBase.Logic;

namespace CodeBase.Infrastructure.States
{
    public class ReloadLevelState : IPayloadedState<string>
    {
        public event Action ReloadLevel;
        private IGameStateMachine _stateMachine;
        private readonly LoadingCurtain _loadingCurtain;
        private IAssetProvider _assets;
        

        public ReloadLevelState(LoadingCurtain loadingCurtain, IGameStateMachine stateMachine)
        {
            _loadingCurtain = loadingCurtain;
            _stateMachine = stateMachine;
        }
        public void Enter(string payload)
        {
            _loadingCurtain.Show();
            ReloadLevel?.Invoke();
            _loadingCurtain.Hide();
            _stateMachine.Enter<GameLoopGameState>();
        }

        public void Exit()
        {
        }
    }
}