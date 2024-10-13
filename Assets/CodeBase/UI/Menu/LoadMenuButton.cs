using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.States;
using CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Menu
{
    public class LoadMenuButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private WindowBase window;
        
        private IGameStateMachine _stateMachine;

        [Inject]
        public void Init(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private void Awake() => 
            button.onClick.AddListener(LoadLevel);

        private void LoadLevel()
        {
                _stateMachine.Enter<LoadMenuState>();
                if(window != null)
                    Destroy(window.gameObject);
        }
    }
}