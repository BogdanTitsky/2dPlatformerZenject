using CodeBase.Infrastructure.Factory;
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
        [SerializeField] private Button Button;
        [SerializeField] private string SceneName;
        
        private IGameStateMachine _stateMachine;

        [Inject]
        public void Init(IGameStateMachine stateMachine) => 
            _stateMachine = stateMachine;

        private void Awake() => 
            Button.onClick.AddListener(LoadLevel);

        private void LoadLevel() => 
            _stateMachine.Enter<LoadLevelState, string>(SceneName);
    }
}