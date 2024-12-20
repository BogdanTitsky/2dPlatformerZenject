using UnityEngine;
using Zenject;

namespace CodeBase.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private HeroAnimator animator;

        private PlayerController _playerController;
        private PlayerStateMachine _stateMachine;

        [Inject]
        public void Construct(PlayerController playerController)
        {
            _playerController = playerController;
            _stateMachine = new PlayerStateMachine(animator);
        }

        private void OnEnable()
        {
            _playerController.PlayerInputActions.Gameplay.Enable();
            _playerController.PlayerInputActions.Gameplay.Attack.performed += _stateMachine.AttackTrigger;
        }

        private void OnDisable()
        {
            _playerController.PlayerInputActions.Gameplay.Attack.performed -= _stateMachine.AttackTrigger;

            _playerController.PlayerInputActions.Gameplay.Disable();
        }

        private void Update() => _stateMachine.Update();

        private void FixedUpdate() => _stateMachine.FixedUpdate();
    }
}