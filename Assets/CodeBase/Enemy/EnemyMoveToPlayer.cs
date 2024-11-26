using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Pause;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    public class EnemyMoveToPlayer : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float speed;
        [SerializeField] private EnemyAnimator animator;
        //TODO apply state machine for this
        [SerializeField] private EnemyAttackBehaviour enemyAttackBehaviour;

        private HeroDeath hero;
        private Transform _targetTransform;
        private IGameFactory _gameFactory;
        private IPauseService _pauseService;
        
        public bool Enabled { get; set; } = true;

        [Inject]
        private void Init(IGameFactory gameFactory, IPauseService pauseService)
        {
            _pauseService = pauseService;
            _gameFactory = gameFactory;
            hero = _gameFactory.HeroDeathObject;
            _targetTransform = _gameFactory.HeroDeathObject.transform;

            hero.OnHeroDeath += HeroDie;
            _pauseService.PauseChanged += OnPauseChanged;
        }

        private void OnDisable()
        {
            hero.OnHeroDeath -= HeroDie;
            _pauseService.PauseChanged -= OnPauseChanged;
        }

        private void OnPauseChanged()
        {
            if (_pauseService.IsPaused)
            {
                _rb.bodyType = RigidbodyType2D.Kinematic;
                _rb.linearVelocity = Vector2.zero;
            }
            else
                _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        private void HeroDie()
        {
           Enabled = false;
           _rb.linearVelocity = Vector2.zero;
           animator.PlayOnHeroDie();
        }

        public void FixedUpdate()
        {
            if (_pauseService.IsPaused)
                return;
            Chase();
            if (enemyAttackBehaviour.InRange)
                _rb.linearVelocity = Vector2.zero;
        }

        private void Chase()
        {
            if (!Enabled) return;

            Vector2 direction = (_targetTransform.position  - _rb.transform.position).normalized;
            LookAtTarget(direction);
            Vector2 velocity = _rb.linearVelocity;
            velocity.x = speed * direction.x;
            _rb.linearVelocity = velocity;
        }

        private void LookAtTarget(Vector2 direction)
        {
            if (direction.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}