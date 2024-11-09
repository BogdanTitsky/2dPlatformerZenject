using System;
using CodeBase.Infrastructure.States;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Windows
{
    public class PauseWindow : WindowBase
    {
        private IGameStateMachine _stateMachine;

        [Inject]
        public void Init(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            _stateMachine.Enter<PauseGameState>();
        }

        protected override void OnWindowClose()
        {
            _stateMachine.Enter<GameLoopState>();
        }
    }
}