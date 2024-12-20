using CodeBase.Infrastructure.Services.Pause;
using UnityEngine;
using Zenject;

namespace CodeBase.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerInputActions PlayerInputActions;
        private IPauseService _pauseService;
        
        [Inject]
        public void Construct(IPauseService pauseService)
        {
            _pauseService = pauseService;
            _pauseService.PauseChanged += OnPauseChange;
        }

        private void OnPauseChange()
        {
            if (_pauseService.IsPaused)
            {
                PlayerInputActions.Gameplay.Disable();
                PlayerInputActions.UI.Enable();
            }
            else
                PlayerInputActions.Gameplay.Enable();
        }

        private void Awake()
        {
            PlayerInputActions = new PlayerInputActions();
            PlayerInputActions.Enable();
        }
    }
}