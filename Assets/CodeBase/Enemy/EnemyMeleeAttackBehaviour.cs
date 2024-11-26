using System;
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
            hitBox.TriggerEnter += CheckPlayerHit;
            hitBoxCollider.enabled = false;
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
            _isAttacking = true;
        }
        
        private void HitBoxOn() => hitBoxCollider.enabled = true;
        
        private void AttackEnd()
        {
            hitBoxCollider.enabled = false;
            _currentAttackCooldown = AttackCooldown;
            _isAttacking = false;
            _uniqueHits.Clear();
        }

        private bool CanAttack() => 
            !_isAttacking && CooldownIsUp() && groundChecker.IsGrounded && InRange; 
        
        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _currentAttackCooldown -= Time.deltaTime;
        }
        
        private bool CooldownIsUp() => 
            _currentAttackCooldown <= 0;
        
    }
}