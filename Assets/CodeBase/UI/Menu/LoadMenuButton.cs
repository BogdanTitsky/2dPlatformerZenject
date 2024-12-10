using CodeBase.Infrastructure.States;
using CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Menu
{
    public class LoadMenuButton : MonoBehaviour
    {
        [SerializeField] private Button Button;
        [SerializeField] private WindowBase Window;
        
        private IGameStateMachine _stateMachine;

        [Inject]
        public void Init(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private void Awake() => 
            Button.onClick.AddListener(LoadLevel);

        private void LoadLevel()
        {
                if(Window != null)
                    Window.gameObject.SetActive(false);
                _stateMachine.Enter<LoadMenuState>();
        }
    }
}