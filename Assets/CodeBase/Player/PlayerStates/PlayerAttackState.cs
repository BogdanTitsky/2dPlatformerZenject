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
        
        private float attackDuration;
        private float elapsedTime;
        private bool isCombo;
        private int currentCombo;

        public override void OnEnter()
        {
            currentCombo = 0;
            Attack();
        }

        public override void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= attackDuration) 
                IsNextAttack();
        }

        public void TryContinueCombo() => 
            isCombo = true;

        private void IsNextAttack()
        {
            if (isCombo)
                Attack();
            else
                AttackEnd?.Invoke();
        }

        private void Attack()
        {
            currentCombo++;
            isCombo = false;
            attackDuration = animator.PlayAttack(currentCombo);
            elapsedTime = 0;
        }
    }
}