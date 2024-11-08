using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] protected Button CloseButton;
        protected IPersistentProgressService _progressService;
        protected PlayerProgress Progress => _progressService.Progress;

        [Inject]
        public void Init(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }
        
        private void Awake() => 
            OnAwake();

        private void OnEnable()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDisable()
        {
            Cleanup();
        }

        protected virtual void OnAwake() => 
            CloseButton.onClick.AddListener(() => Destroy(gameObject));

        protected virtual void Initialize()
        {
            
        }

        protected virtual void SubscribeUpdates()
        {
            
        }

        protected virtual void Cleanup()
        {
            
        }
    }
}