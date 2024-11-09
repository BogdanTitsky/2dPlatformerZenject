using System;
using System.Collections.Generic;
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
                [typeof(BootstrapState)] = container.Resolve<BootstrapState>(),
                [typeof(LoadMenuState)] = container.Resolve<LoadMenuState>(),
                [typeof(LoadLevelState)] = container.Resolve<LoadLevelState>(),
                [typeof(LoadProgressState)] = container.Resolve<LoadProgressState>(),
                [typeof(GameLoopState)] = container.Resolve<GameLoopState>(),
                [typeof(ReloadLevelState)] = container.Resolve<ReloadLevelState>(),
                [typeof(PauseGameState)] = container.Resolve<PauseGameState>(),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
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