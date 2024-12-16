using System.Collections;
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
        [SerializeField] private TriggerObserver attackRange;
        
        private bool CooldownIsUp => _currentAttackCooldown >= AttackCooldown;
        public float AttackCooldown = 1f;
        public float Damage = 5f;
        private float _currentAttackCooldown;

        public bool InRange { get; set; }

        private IPauseService _pauseService;
        protected IHealth _heroHealth;
        protected int layerMask;

        protected IGameFactory _gameFactory;
        private Coroutine _cooldownCoroutine;

        [Inject]
        public void Init(IGameFactory gameFactory, IPauseService pauseService)
        {
            _pauseService = pauseService;
            _gameFactory = gameFactory;
            _heroHealth = _gameFactory.HeroDeathObject.GetComponent<IHealth>();
        }

        private void Awake()
        {
            _currentAttackCooldown = AttackCooldown;
            layerMask = LayerMask.NameToLayer("Player");
        }

        protected virtual void OnEnable()
        {
            animator.StateExited += CheckStateExited;
            attackRange.TriggerEnter += TriggerEnter;
            attackRange.TriggerExit += TriggerExit;
        }
        
        protected virtual void OnDisable()
        {
            animator.StateExited -= CheckStateExited;
            attackRange.TriggerEnter -= TriggerEnter;
            attackRange.TriggerExit -= TriggerExit;
        }

        private void TriggerExit(Collider2D obj) => InRange = false;

        private void TriggerEnter(Collider2D obj) => InRange = true;


        public void OnUpdate()
        {
            if (_pauseService.IsPaused || !CooldownIsUp)
                return;
            _currentAttackCooldown = 0;
            StartCooldownTimer();
            animator.Attack();
        }

        protected abstract void CheckStateExited(AnimatorState obj);

        public bool CanAttack() => groundChecker.IsGrounded && InRange;

        private void StartCooldownTimer()
        {
            if (_cooldownCoroutine != null)
                StopCoroutine(_cooldownCoroutine);

            _cooldownCoroutine = StartCoroutine(CooldownTimer());
        }

        private IEnumerator CooldownTimer()
        {
            while (_currentAttackCooldown < AttackCooldown)
            {
                if (!_pauseService.IsPaused)
                    _currentAttackCooldown += Time.deltaTime;

                yield return null;
            }
        }
    }
}