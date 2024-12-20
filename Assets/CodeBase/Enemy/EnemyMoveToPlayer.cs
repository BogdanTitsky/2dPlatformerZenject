using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Pause;
using CodeBase.Player;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    public class EnemyMoveToPlayer : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float speed;
        [SerializeField] private EnemyAnimator animator;
        [SerializeField] private EnemyAttackBehaviour enemyAttackBehaviour;
        [SerializeField] private GroundChecker groundChecker;
        [SerializeField] private float jumpForce = 50;
        [SerializeField] private LayerMask ground;
        
        private Transform _targetTransform;
        private IGameFactory _gameFactory;
        private IPauseService _pauseService;
        
        [Inject]
        private void Init(IGameFactory gameFactory, IPauseService pauseService)
        {
            _pauseService = pauseService;
            _gameFactory = gameFactory;
            _targetTransform = _gameFactory.HeroDeathObject.transform;

            _pauseService.PauseChanged += OnPauseChanged;
        }

        private void OnDisable()
        {
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

        public void Chase()
        {
            if (_pauseService.IsPaused) return;

            Vector2 direction = GetDirection();
            LookAtTarget();

            if (CheckWalls() && groundChecker.IsGrounded)
            {
                Jump();
            }
            
            Vector2 velocity = _rb.linearVelocity;
            velocity.x = speed * direction.x;
            _rb.linearVelocity = velocity;
        }

        private bool CheckWalls()
        {
            Vector3 direction = new(transform.localScale.x, 0);
            RaycastHit2D hit = Physics2D.Raycast(_rb.position,direction , 2,ground);
            return hit.collider;
        }
        
        private void Jump() => 
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);

        public void LookAtTarget()
        {
            Vector2 direction = GetDirection();
            if (direction.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }
        
        private Vector2 GetDirection()=>(_targetTransform.position  - _rb.transform.position).normalized;

        public void StopMove() => _rb.linearVelocity = Vector2.zero;
    }
}