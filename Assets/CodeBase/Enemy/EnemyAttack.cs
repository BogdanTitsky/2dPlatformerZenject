using System;
using System.Linq;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator animator;
        [SerializeField] private int layerMask;
        public float Cleavage = 1f;
        public Vector2 Distance = new ();
        public float AttackCooldown = 1.5f;
        public float Damage = 5f;
        
        
        private Collider2D[] _hits = new Collider2D[1];
        private float _currentAttackCooldown;
        private bool _isAttacking;
        private bool _attackIsActive;
        
        private IGameFactory _gameFactory;
        private IHealth _heroHealth;

        [Inject]
        public void Init(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _heroHealth = _gameFactory.HeroGameObject.GetComponent<IHealth>();
        }

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
               PhysicsDebug.DrawDebug(StartPoint(), Cleavage, 2);
               _heroHealth.TakeDamage(Damage);
            }
        }
        
        private void OnAttackEnd()
        {
            _currentAttackCooldown = AttackCooldown;
            _isAttacking = false;
        }

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _currentAttackCooldown -= Time.deltaTime;
        }

        private bool Hit(out Collider2D hit)
        {
            int hitCount = Physics2D.OverlapBoxNonAlloc(StartPoint(), Size(), layerMask ,_hits);

            hit = _hits.FirstOrDefault();

            return hitCount > 0;
        }

        private Vector2 Size() => new(Cleavage, Cleavage);
          
        private Vector2 StartPoint() => new Vector2(transform.position.x, transform.position.y) + Distance;

        private bool CanAttack() => 
            !_isAttacking && CooldownIsUp() && _attackIsActive;

        private bool CooldownIsUp() => 
            _currentAttackCooldown <= 0;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(StartPoint(), Size());
        }
    }
}