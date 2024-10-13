﻿using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.States;
using CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Menu
{
    public class LoadSceneButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private string sceneName;
        [SerializeField] private WindowBase window;
        
        private IGameStateMachine _stateMachine;
        private ISaveLoadService _saveLoadService;
        private IGameFactory _gameFactory;

        [Inject]
        public void Init(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private void Awake() => 
            button.onClick.AddListener(LoadLevel);

        private void LoadLevel()
        {
            _stateMachine.Enter<LoadLevelState, string>(sceneName);
            if(window != null)
                Destroy(window.gameObject);
        }
    }
}