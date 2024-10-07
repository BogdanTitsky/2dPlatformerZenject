using System;
using CodeBase.Data;
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

        private IInputService _inputService;
        private Camera _camera;
        private Vector3 _inputDirection;
        [Inject]
        public void Init(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Start() =>
            _camera = Camera.main;

        private void Update()
        {
            _inputDirection = Vector2.zero;

            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                _inputDirection = _camera.transform.TransformDirection(_inputService.Axis);
                _inputDirection.y = 0;

                LookAtMoveDirection();
            }
        }

        private void FixedUpdate()
        {
            Vector2 velocity = _rigidbody2D.velocity;
            velocity.x = _movementSpeed * _inputDirection.x;
            _rigidbody2D.velocity = velocity;
        }

        public void StopMoving()
        {
            _rigidbody2D.velocity = Vector2.zero;
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