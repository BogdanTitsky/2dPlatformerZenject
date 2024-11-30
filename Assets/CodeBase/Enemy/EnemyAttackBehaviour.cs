using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Pause;
using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    public abstract class EnemyAttackBehaviour : MonoBehaviour
    {
        [SerializeField] protected EnemyAnimator animator;
        [SerializeField] protected GroundChecker groundChecker;
        
        public float AttackCooldown = 1.5f;
        public float Damage = 5f;
        public bool InRange { get; set; }
        public IEnemyStateMachine StateMachine;

        private IPauseService _pauseService;
        protected IHealth _heroHealth;
        protected int layerMask;

        protected IGameFactory _gameFactory;

        protected float _currentAttackCooldown;

        [Inject]
        public void Init(IGameFactory gameFactory, IPauseService pauseService)
        {
            _pauseService = pauseService;
            _gameFactory = gameFactory;
            _heroHealth = _gameFactory.HeroDeathObject.GetComponent<IHealth>();
        }

        private void Awake() => 
            layerMask = LayerMask.NameToLayer("Player");
        
        protected virtual void OnEnable()
        {
            animator.StateExited += CheckStateExited;
        }

        protected virtual void OnDisable()
        {
            animator.StateExited -= CheckStateExited;
        }

        protected virtual void Update()
        {
            if (_pauseService.IsPaused)
                return;
            
            UpdateCooldown();
            if (CanAttack()) 
                StartAttack();
        }

        protected abstract void CheckStateExited(AnimatorState obj);

        protected abstract void StartAttack();

        
        protected abstract bool CanAttack();

        protected bool CooldownIsUp() => 
            _currentAttackCooldown <= 0;

        private void UpdateCooldown()
        {
            if (!CooldownIsUp())
                _currentAttackCooldown -= Time.deltaTime;
        }
    }
}