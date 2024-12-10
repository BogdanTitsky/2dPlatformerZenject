using System;
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

        public float AttackCooldown = 1f;
        public float Damage = 5f;

        private bool _inRange;
        public bool InRange
        {
            get => _inRange;
            set
            {
                if (_inRange == value) return;
                _inRange = value;
                InRangeChanged();
            }
        }

        private void InRangeChanged()
        {
            if (!InRange) animator.PlayAttacking(false);
        }

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
        
        //Animator event
        public void ResetCooldown()
        {
            _currentAttackCooldown = AttackCooldown;
            animator.PlayAttacking(false);
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

        private void StartAttack() => animator.PlayAttacking(true);

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