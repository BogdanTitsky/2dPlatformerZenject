using System.Collections.Generic;
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
        
        protected IPauseService _pauseService;
        protected IHealth _heroHealth;
        protected int layerMask;
        protected readonly HashSet<Collider2D> _uniqueHits = new();

        private IGameFactory _gameFactory;

        [Inject]
        public void Init(IGameFactory gameFactory, IPauseService pauseService)
        {
            _pauseService = pauseService;
            _gameFactory = gameFactory;
            _heroHealth = _gameFactory.HeroDeathObject.GetComponent<IHealth>();
        }

        private void Awake() => 
            layerMask = LayerMask.NameToLayer("Player");
    }
}