using CodeBase.Infrastructure.States;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private Game _game;

        [Inject]
        private void Init(Game game)
        {
            _game = game;
        }

        private void Awake()
        {
            _game.StateMachine.Enter<BootstrapState>();
        }
    }
}