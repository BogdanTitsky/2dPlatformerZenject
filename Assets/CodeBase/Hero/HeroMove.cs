using CodeBase.Data;
using CodeBase.Infrastructure.Services.Pause;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Hero
{
    public class HeroMove : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private HeroAnimator heroAnimator;
        [SerializeField] private float movementSpeed;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private GameObject nonRotatable;
        private bool AbleMove = true;
        private IInputService _inputService;
        private Vector3 _inputDirection;
        private IPauseService _pauseService;
        private Vector3 mirrored = new(-1, 1, 1);

        [Inject]
        public void Init(IInputService inputService, IPauseService pauseService)
        {
            _pauseService = pauseService;
            _inputService = inputService;
            _pauseService.PauseChanged += OnPauseChanged;
        }
        private void OnEnable()
        {
            heroAnimator.StateEntered += CheckEnteredState;
        }

        private void OnDisable()
        {
            heroAnimator.StateEntered -= CheckEnteredState;
            _pauseService.PauseChanged -= OnPauseChanged;
        }

        private void Update()
        {
            if (_pauseService.IsPaused)
                return;
            
            _inputDirection = Vector2.zero;

            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon && AbleMove)
            {
                _inputDirection = _inputService.Axis;
                _inputDirection.y = 0;
                LookAtMoveDirection();
            }
        }

        private void FixedUpdate()
        {
            if (_pauseService.IsPaused)
                return;
            
            Vector2 velocity = rb.linearVelocity;
            velocity.x = movementSpeed * _inputDirection.x;
            rb.linearVelocity = velocity;
        }

        private void CheckEnteredState(AnimatorState state)
        {
            switch (state)
            {
                case AnimatorState.Attack:
                case AnimatorState.SecondAttack:
                case AnimatorState.ThirdAttack:
                case AnimatorState.Block: 
                case AnimatorState.Died:
                case AnimatorState.Stunned:
                    MoveOff();
                    break;
                case AnimatorState.Hurt:
                    AbleMove = false;
                    break;        
                default:
                    MoveOn();
                    break;
            }
        }
        
        private void MoveOff()
        {
            AbleMove = false;
            rb.linearVelocity = Vector2.zero;
        }

        private void MoveOn() => AbleMove = true;

        public void KnockUp()
        {
            rb.AddForce(Vector2.up * 30, ForceMode2D.Impulse);
        }

        private void OnPauseChanged() => 
            rb.bodyType = _pauseService.IsPaused ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;

        public void TurnOffHeroMove() => rb.linearVelocity = Vector2.zero;

        private void LookAtMoveDirection()
        {
            if (_inputDirection.x > 0)
            {
                transform.localScale = Vector3.one;
                nonRotatable.transform.localScale = Vector3.one;
            }
            else if (_inputDirection.x < 0)
            {
                transform.localScale = mirrored;
                nonRotatable.transform.localScale = mirrored;
            }
        }
        
        #region Progress
        
        public void UpdateProgress(PlayerProgress progress) => 
            progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());

        public void LoadProgress(PlayerProgress progress)
        {
            if (CurrentLevel() != progress.WorldData.PositionOnLevel.SceneName) return;

            Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
            if (savedPosition != null)
                transform.position = savedPosition.AsUnityVector();
        }
        
        private static string CurrentLevel() =>
            SceneManager.GetActiveScene().name;
        
        #endregion
        
    }
}