using System;
using UnityEngine;

namespace CodeBase.Player.PlayerStates
{
    public class PlayerAttackState : PlayerBaseState
    {
        public PlayerAttackState(HeroAnimator animator) : base(animator)
        {
        }

        public event Action AttackEnd;

        private float _attackDuration;
        private float _elapsedTime;
        private bool _isCombo;
        private int _currentCombo;

        public override void OnEnter()
        {
            _currentCombo = 0;
            Attack();
        }

        public override void OnUpdate()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _attackDuration) 
                IsNextAttack();
        }

        public void TryContinueCombo() => 
            _isCombo = true;

        private void IsNextAttack()
        {
            if (_isCombo)
                Attack();
            else
                AttackEnd?.Invoke();
        }

        private void Attack()
        {
            _currentCombo++;
            _isCombo = false;
            _attackDuration = animator.CalculatePlayAttackDuration(_currentCombo);
            _elapsedTime = 0;
        }
    }
}