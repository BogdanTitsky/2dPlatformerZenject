using CodeBase.Infrastructure.States;
using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Services.Windows
{
    public class UIRootCanvas : MonoBehaviour
    {
        private IGameStateMachine _stateMachine;
        private IInputService _inputService;
        private IWindowService _windowService;

        [Inject]
        public void Init(IGameStateMachine stateMachine, IInputService inputService, IWindowService windowService)
        {
            _stateMachine = stateMachine;
            _inputService = inputService;
            _windowService = windowService;
        }

        private void Update()
        {
            if (_inputService.IsEscapeButtonDown())
            {
                if (_stateMachine.GetActiveStateType == typeof(GameLoopState))
                {
                   _windowService.Open(WindowId.Pause);
                }else if (_stateMachine.GetActiveStateType == typeof(PauseGameState))
                    _windowService.Hide(WindowId.Pause);
            }
        }
    }
}
