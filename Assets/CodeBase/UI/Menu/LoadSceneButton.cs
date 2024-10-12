using CodeBase.Infrastructure.Services.PersistentProgress;
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
        
        private IPersistentProgressService _progressService;
        private IGameStateMachine _stateMachine;

        [Inject]
        public void Init(IGameStateMachine stateMachine, IPersistentProgressService progressService)
        {
            _stateMachine = stateMachine;
            _progressService = progressService;
        }

        private void Awake() => 
            button.onClick.AddListener(LoadLevel);

        private void LoadLevel()
        {
            if (sceneName == "Menu")
                _stateMachine.Enter<LoadMenuState>();
            else
                _stateMachine.Enter<LoadLevelState, string>(sceneName);
            if(window != null)
                Destroy(window.gameObject);
        }
    }
}