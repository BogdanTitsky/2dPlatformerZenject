using CodeBase.Infrastructure.Services.Pause;
using CodeBase.Services.Input;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField] private WindowId windowId;
    
        private IWindowService _windowService;
        private IInputService _inputService;
        private IPauseService _pauseService;

        [Inject]
        public void Init(IWindowService windowService, IInputService inputService, IPauseService pauseService)
        {
            _pauseService = pauseService;
            _inputService = inputService;
            _windowService = windowService;
        }

        private void Update()
        {
            if (!_inputService.IsEscapeButtonDown()) return;
            
            if (_pauseService.IsPaused)
                _windowService.Hide(windowId);
            else
                _windowService.Open(windowId);
        }
    }
}
