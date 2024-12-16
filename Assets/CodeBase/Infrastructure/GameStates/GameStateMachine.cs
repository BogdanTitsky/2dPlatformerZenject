using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.GameStates;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        public Type GetActiveStateType => _activeState?.GetType();
        
        private Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;
        
        [Inject]
        public void Init(DiContainer container)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapGameState)] = container.Resolve<BootstrapGameState>(),
                [typeof(LoadProgressGameState)] = container.Resolve<LoadProgressGameState>(),
                [typeof(InitGameGameState)] = container.Resolve<InitGameGameState>(),
                [typeof(LoadMenuGameState)] = container.Resolve<LoadMenuGameState>(),
                [typeof(LoadLevelState)] = container.Resolve<LoadLevelState>(),
                [typeof(GameLoopGameState)] = container.Resolve<GameLoopGameState>(),
                [typeof(ReloadLevelState)] = container.Resolve<ReloadLevelState>(),
            };
        }

        public void Enter<TState>() where TState : class, IGameState
        {
            IGameState gameState = ChangeState<TState>();
            gameState.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}