using System;
using System.Linq;
using CodeBase.Hero;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class Attack : MonoBehaviour
    {
        [SerializeField] private float cleavage = 1f;
        [SerializeField] private float range = 1f;
        [SerializeField] private int layerMask;
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private EnemyAnimator animator;
        [SerializeField] private float damage = 5f;
        
        
        private Collider2D[] _hits = new Collider2D[1];
        private float _currentAttackCooldown;
        private bool _isAttacking;
        private bool _attackIsActive;


        private void Awake()
        {
            layerMask = 1 << LayerMask.NameToLayer("Player");
        }

        private void Update()
        {
            UpdateCooldown();
            if (CanAttack())
                StartAttack();
        }

        public void EnableAttack() => 
            _attackIsActive = true;

        public void DisableAttack() => 
            _attackIsActive = false;

        private void StartAttack()
        {
            animator.PlayAttack();
            _isAttacking = true;
        }
        
        private void OnAttack()
        {
            if (Hit(out Collider2D hit))
            {
               PhysicsDebug.DrawDebug(StartPoint(), cleavage, 2);
               hit.transform.GetComponent<HeroHealth>().TakeDamage(damage);
            }
        }
        
        private void OnAttackEnd()
        {
            _currentAttackCooldown = attackCooldown;
            _isAttacking = false;
        }

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _currentAttackCooldown -= Time.deltaTime;
        }

        private bool Hit(out Collider2D hit)
        {
            Vector2 startPoint = StartPoint();
            
            Vector2 size = Size();

            int hitCount = Physics2D.OverlapCapsuleNonAlloc(startPoint, size,
                CapsuleDirection2D.Vertical,  layerMask,_hits);

            hit = _hits.FirstOrDefault();

            return hitCount > 0;
        }

        private Vector2 Size()
        {
            float radius = cleavage / 2; 
            float height = cleavage;
            Vector2 size = new Vector2(radius, height);
            return size;
        }

        private Vector2 StartPoint()
        {
            return new Vector2(transform.position.x, transform.position.y) +
                   new Vector2(transform.localScale.x, transform.localScale.y) * range;
        }
        
        private bool CanAttack() => 
            !_isAttacking && CooldownIsUp() && _attackIsActive;

        private bool CooldownIsUp() => 
            _currentAttackCooldown <= 0;

        
    }
}