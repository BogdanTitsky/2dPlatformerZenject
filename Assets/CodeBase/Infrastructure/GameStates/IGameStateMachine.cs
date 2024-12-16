using System;
using CodeBase.Infrastructure.GameStates;

namespace CodeBase.Infrastructure.States
{
    public interface IGameStateMachine
    {
        void Enter<TState>() where TState : class, IGameState;
        void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
        Type GetActiveStateType { get; }
    }
}