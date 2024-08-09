using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        [Inject] public GameStateMachine StateMachine;
    }
}