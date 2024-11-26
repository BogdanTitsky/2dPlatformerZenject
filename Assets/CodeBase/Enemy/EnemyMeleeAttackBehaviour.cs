using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyMeleeAttackBehaviour : EnemyAttackBehaviour
    {
        [SerializeField] private TriggerObserver hitBox;
        [SerializeField] private Collider2D hitBoxCollider;

        private float _currentAttackCooldown;

        private void Start()
        {
            animator.StateExited += CheckStateExited;
            hitBox.TriggerEnter += CheckPlayerHit;
            hitBoxCollider.enabled = false;
        }

        private void CheckStateExited(AnimatorState obj)
        {
            if (obj == AnimatorState.Attack)
            {
                _currentAttackCooldown = AttackCooldown;
                _uniqueHits.Clear();
            }
        }

        private void OnDisable() => 
            hitBox.TriggerEnter -= CheckPlayerHit;

        private void Update()
        {
            if (_pauseService.IsPaused)
                return;
            
            UpdateCooldown();
            if (CanAttack()) 
                StartAttack();
        }
        
        private void CheckPlayerHit(Collider2D other)
        {
            if (other.gameObject.layer == layerMask)
            {
                _uniqueHits.Add(other);
                _heroHealth.TakeDamage(Damage);
            }
        }

        private void StartAttack()
        {
            animator.PlayAttack();
        }
        
        private void HitBoxOn() => hitBoxCollider.enabled = true;
        
        private void HitBoxOff() => hitBoxCollider.enabled = false;

        private bool CanAttack() => 
                            animator.State != AnimatorState.Attack 
                            && CooldownIsUp() && groundChecker.IsGrounded && InRange; 
        
        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _currentAttackCooldown -= Time.deltaTime;
        }
        
        private bool CooldownIsUp() => 
            _currentAttackCooldown <= 0;
        
    }
}