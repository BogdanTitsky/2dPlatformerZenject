using CodeBase.Infrastructure.States;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        [Inject] public GameStateMachine StateMachine;
    }
}