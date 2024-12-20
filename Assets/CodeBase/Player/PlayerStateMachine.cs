using CodeBase.Infrastructure.StateMachine;
using CodeBase.Player.PlayerStates;
using UnityEngine.InputSystem;

namespace CodeBase.Player
{
    public class PlayerStateMachine
    {
        private readonly EventStateMachine _stateMachine = new();
        private readonly PlayerAttackState _attackState;
        private readonly PlayerIdleState _idleState;

        public PlayerStateMachine(HeroAnimator animator)
        {
            //State initialization
            _attackState = new PlayerAttackState(animator);
            _idleState = new PlayerIdleState(animator);

            _attackState.AttackEnd += OnAttackEnd;

            //Possible transitions
            At(_idleState, _attackState);
            At(_attackState, _idleState);

            _stateMachine.SetInitialState(_idleState);
        }

        ~PlayerStateMachine()
        {
            _attackState.AttackEnd -= OnAttackEnd;
        }

        public void Update() => _stateMachine.Update();

        public void FixedUpdate() => _stateMachine.FixedUpdate();

        public void AttackTrigger(InputAction.CallbackContext obj)
        {
            if (_stateMachine.CurrentState() == _attackState) 
                _attackState.TryContinueCombo();
            else
                _stateMachine.ChangeState(_attackState);
        }

        private void OnAttackEnd()
        {
            _stateMachine.ChangeState(_idleState);
        }

        #region Helpers

        private void At(IState from, IState to) =>
            _stateMachine.AddTransition(from, to);

        private void Any(IState to, IPredicate condition) =>
            _stateMachine.AddAnyTransition(to);

        #endregion
    }
}