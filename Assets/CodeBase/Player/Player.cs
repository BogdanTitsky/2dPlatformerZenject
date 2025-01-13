using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace CodeBase.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private HeroAnimator animator;
        [SerializeField] private HeroMove heroMove;

        private PlayerController _playerController;
        private PlayerStateMachine _stateMachine;

        [Inject]
        public void Construct(PlayerController playerController)
        {
            _playerController = playerController;
            _stateMachine = new PlayerStateMachine(animator, heroMove);
        }

        private void OnEnable()
        {
            _playerController.PlayerInputActions.Gameplay.Enable();
            _playerController.PlayerInputActions.Gameplay.Attack.performed += AttackOnPerformed;
            _playerController.PlayerInputActions.Gameplay.Move.performed += MoveOnPerformed;
            _playerController.PlayerInputActions.Gameplay.Move.canceled += MoveOnCanceled;
        }

        

        private void OnDisable()
        {
            _playerController.PlayerInputActions.Gameplay.Attack.performed -= AttackOnPerformed;
            _playerController.PlayerInputActions.Gameplay.Move.performed -= MoveOnPerformed;
            _playerController.PlayerInputActions.Gameplay.Move.canceled -= MoveOnCanceled;

            _playerController.PlayerInputActions.Gameplay.Disable();
        }

        private void AttackOnPerformed(InputAction.CallbackContext obj)
        {
            _stateMachine.AttackTrigger();
        }

        private void MoveOnPerformed(InputAction.CallbackContext context)
        {
            _stateMachine.MoveTrigger(context);
        }
        
        private void MoveOnCanceled(InputAction.CallbackContext obj)
        {
            _stateMachine.MoveCanceled();
        }

        private void Update() => _stateMachine.Update();

        private void FixedUpdate() => _stateMachine.FixedUpdate();
    }
}