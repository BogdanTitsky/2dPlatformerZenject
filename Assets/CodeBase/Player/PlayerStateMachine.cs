using CodeBase.Infrastructure.StateMachine;
using CodeBase.Player.PlayerStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Player
{
    public class PlayerStateMachine
    {
        private readonly EventStateMachine _stateMachine = new();
        private readonly PlayerAttackState _attackState;
        private readonly PlayerIdleState _idleState;
        private readonly PlayerWalkingState _walkingState;

        public PlayerStateMachine(HeroAnimator animator, HeroMove heroMove)
        {
            //State initialization
            _attackState = new PlayerAttackState(animator);
            _idleState = new PlayerIdleState(animator);
            _walkingState = new PlayerWalkingState(animator, heroMove);

            _attackState.AttackEnd += OnAttackEnd;

            //Possible transitions
            At(_idleState, _attackState);
            At(_idleState, _walkingState);
            At(_walkingState, _attackState);
            At(_walkingState, _idleState);
            At(_attackState, _idleState);

            _stateMachine.SetInitialState(_idleState);
        }

        ~PlayerStateMachine()
        {
            _attackState.AttackEnd -= OnAttackEnd;
        }

        public void Update() => _stateMachine.Update();

        public void FixedUpdate() => _stateMachine.FixedUpdate();

        public void AttackTrigger()
        {
            if (_stateMachine.CurrentState() == _attackState) 
                _attackState.TryContinueCombo();
            else
                _stateMachine.ChangeState(_attackState);
        }
        
        public void MoveTrigger(InputAction.CallbackContext context)
        {
            _stateMachine.ChangeState(_walkingState);
            _walkingState.MoveDirection = context.ReadValue<Vector2>();
        }
        
        public void MoveCanceled()
        {
            _walkingState.MoveDirection = Vector2.zero;
            if (_stateMachine.CurrentState() != _attackState) 
                _stateMachine.ChangeState(_idleState);
        }
        
        private void OnAttackEnd()
        {
            _stateMachine.ChangeState(_idleState);
            if (Mathf.Abs(_walkingState.MoveDirection.x) >= Constants.Epsilon) 
                _stateMachine.ChangeState(_walkingState);
        }

        #region Helpers

        private void At(IState from, IState to) =>
            _stateMachine.AddTransition(from, to);

        private void Any(IState to, IPredicate condition) =>
            _stateMachine.AddAnyTransition(to);

        #endregion


    }
}