using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class OpenWindowButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private WindowId windowId;
    
        private IWindowService _windowService;
    
        [Inject]
        public void Init(IWindowService windowService)
        {
            _windowService = windowService;
        }

        private void Awake() => 
            button.onClick.AddListener(Open);

        private void Open() => 
            _windowService.Open(windowId);
    }
}
