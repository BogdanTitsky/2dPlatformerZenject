using CodeBase.Infrastructure.Services.Pause;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField] private WindowId windowId;
        [SerializeField] private Button button;
        
        
        private IWindowService _windowService;

        [Inject]
        public void Init(IWindowService windowService, IPauseService pauseService)
        {
            _windowService = windowService;
        }

        private void Awake() => 
            button.onClick.AddListener((() => _windowService.Open(windowId)));
    }
}
