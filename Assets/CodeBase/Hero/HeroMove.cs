using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.Pause;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Hero
{
    public class HeroMove : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        public bool AbleMove = true;
        private IInputService _inputService;
        private Vector3 _inputDirection;
        private IPauseService _pauseService;

        [Inject]
        public void Init(IInputService inputService, IPauseService pauseService)
        {
            _pauseService = pauseService;
            _inputService = inputService;
            _pauseService.PauseChanged += OnPauseChanged;
        }

        private void OnDisable() => 
            _pauseService.PauseChanged -= OnPauseChanged;

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
            
            Vector2 velocity = _rigidbody2D.linearVelocity;
            velocity.x = _movementSpeed * _inputDirection.x;
            _rigidbody2D.linearVelocity = velocity;
        }

        public void MoveOff() => AbleMove = false;
        
        public void MoveOn() => AbleMove = true;

        private void OnPauseChanged()
        {
            if (_pauseService.IsPaused)
            {
                _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                _rigidbody2D.linearVelocity = Vector2.zero;
            }
            else
                _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }

        private void LookAtMoveDirection()
        {
            if (_inputDirection.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (_inputDirection.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (CurrentLevel() != progress.WorldData.PositionOnLevel.SceneName) return;

            Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
            if (savedPosition != null)
                transform.position = savedPosition.AsUnityVector();
        }

        private static string CurrentLevel() =>
            SceneManager.GetActiveScene().name;
    }
}