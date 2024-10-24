using CodeBase.Infrastructure.States;
using CodeBase.UI.Windows;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Menu
{
    public class ReloadLevelButton : MonoBehaviour
    {
        [SerializeField] private Button Button;
        [SerializeField] private WindowBase Window;

        private IGameStateMachine _stateMachine;

        [Inject]
        public void Init(IGameStateMachine stateMachine) => 
            _stateMachine = stateMachine;

        private void Awake() => 
            Button.onClick.AddListener(ReloadLevel);

        private void ReloadLevel()
        {
            _stateMachine.Enter<ReloadLevelState, string>(SceneManager.GetActiveScene().name);
            if(Window != null)
                Destroy(Window.gameObject);
        }
    }
}