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

        private IInputService _inputService;
        private Camera _camera;

        [Inject]
        public void Init(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Start() =>
            _camera = Camera.main;

        private void Update()
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                movementVector = _camera.transform.TransformDirection(_inputService.Axis);
                movementVector.z = 0;
                movementVector.y = 0;
               
                if (movementVector.x > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else if (movementVector.x < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1); 
                }
            }

            transform.position += _movementSpeed * movementVector * Time.deltaTime;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (CurrentLevel() != progress.WorldData.PositionOnLevel.Level) return;

            Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
            if (savedPosition != null)
                transform.position = savedPosition.AsUnityVector();
        }

        private static string CurrentLevel() =>
            SceneManager.GetActiveScene().name;
    }
}