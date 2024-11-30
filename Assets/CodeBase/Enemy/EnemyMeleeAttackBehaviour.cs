using System.Collections.Generic;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyMeleeAttackBehaviour : EnemyAttackBehaviour
    {
        [SerializeField] private TriggerObserver hitBox;
        [SerializeField] private Collider2D hitBoxCollider;
        private readonly HashSet<Collider2D> _uniqueHits = new();

        protected override void OnEnable()
        {
            base.OnEnable();
            hitBox.TriggerEnter += CheckPlayerHit;
            hitBoxCollider.enabled = false;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            hitBox.TriggerEnter -= CheckPlayerHit;
        }

        protected override void CheckStateExited(AnimatorState obj)
        {
            if (obj == AnimatorState.Attack)
            {
                _currentAttackCooldown = AttackCooldown;
                _uniqueHits.Clear();
            }
        }

        private void CheckPlayerHit(Collider2D other)
        {
            if (other.gameObject.layer == layerMask)
            {
                _uniqueHits.Add(other);
                _heroHealth.TakeDamage(Damage);
            }
        }

        protected override void StartAttack()
        {
            animator.PlayAttack();
        }

        protected override bool CanAttack() =>
            animator.State != AnimatorState.Attack
            && CooldownIsUp() && groundChecker.IsGrounded && InRange;

        private void HitBoxOn() => hitBoxCollider.enabled = true;
        
        private void HitBoxOff() => hitBoxCollider.enabled = false;

     
    }
}