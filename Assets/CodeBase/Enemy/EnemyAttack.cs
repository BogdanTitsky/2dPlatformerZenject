using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Pause;
using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator animator;
        [SerializeField] private GroundChecker groundChecker;

        public bool InRange { get; set; }
        public float Cleavage = 1f;
        public Vector2 Distance;
        public float AttackCooldown = 1.5f;
        public float Damage = 5f;
        
        private float _currentAttackCooldown;
        private bool _isAttacking;
        
        private IGameFactory _gameFactory;
        private IHealth _heroHealth;
        private int layerMask;
        
        private IPauseService _pauseService;


        [Inject]
        public void Init(IGameFactory gameFactory, IPauseService pauseService)
        {
            _pauseService = pauseService;
            _gameFactory = gameFactory;
            _heroHealth = _gameFactory.HeroDeathObject.GetComponent<IHealth>();
        }

        private void Awake()
        {
            layerMask = 1 << LayerMask.NameToLayer("Player");
        }

        private void Update()
        {
            if (_pauseService.IsPaused)
                return;
            
            UpdateCooldown();
            if (InRange && CanAttack()) 
                StartAttack();
        }

        private void StartAttack()
        { 
            animator.PlayAttack();
            _isAttacking = true;
        }
        
        private void InflictDamage()
        {
            if (Hit()) 
                _heroHealth.TakeDamage(Damage);
        }
        
        private void OnAttackEnd()
        {
            _currentAttackCooldown = AttackCooldown;
            _isAttacking = false;
        }
        
        private bool CanAttack() => 
            !_isAttacking && CooldownIsUp()  && groundChecker.IsGrounded;
        
        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _currentAttackCooldown -= Time.deltaTime;
        }
        
        private bool CooldownIsUp() => 
            _currentAttackCooldown <= 0;
        
        private bool Hit()
        {
            Collider2D hit = Physics2D.OverlapBox(StartPoint(), Size(), 0, layerMask);
            return hit != null;
        }

        private Vector2 Size() => new(Cleavage, Cleavage);
          
        private Vector2 StartPoint() => new Vector2(transform.position.x, transform.position.y) 
                                        + Distance * transform.localScale;
 
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(StartPoint(), Size());
        }
    }
}