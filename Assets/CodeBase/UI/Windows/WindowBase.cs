using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] protected Button closeButton;
        protected IPersistentProgressService _progressService;
        protected IWindowService _windowService;
        protected PlayerProgress Progress => _progressService.Progress;
        
        [Inject]
        public void Init(IPersistentProgressService progressService, IWindowService windowService)
        {
            _progressService = progressService;
            _windowService = windowService;
        }
        
        private void Awake() => 
            OnAwake();

        private void OnEnable()
        {
            OnEnableWindow();
            SubscribeUpdates();
        }

        private void OnDisable()
        {
            OnWindowClose();
        }

        protected virtual void OnAwake()
        {
            closeButton.onClick.AddListener(HideWindow);
        }

        private void HideWindow()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnEnableWindow()
        {
            
        }

        protected virtual void SubscribeUpdates()
        {
            
        }
        protected virtual void OnWindowClose()
        {
            
        }
    }
}