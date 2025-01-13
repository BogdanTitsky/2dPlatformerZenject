using CodeBase.Data;
using CodeBase.Infrastructure.Services.Pause;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Player
{
    public class HeroMove : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private HeroAnimator heroAnimator;
        [SerializeField] private float movementSpeed;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private GameObject nonRotatable;
        private Vector3 _inputDirection;
        private IPauseService _pauseService;
        private Vector3 mirrored = new(-1, 1, 1);

        public void Move(Vector2 direction)
        {
            _inputDirection = Vector2.zero;
            if (direction.sqrMagnitude > Constants.Epsilon)
            {
                _inputDirection.x = direction.x;
                LookAtMoveDirection();
            }
        }

        public void ApplyMove()
        {
            rb.linearVelocityX =  movementSpeed * _inputDirection.x;
        }

        public void MoveOff()
        {
            rb.linearVelocity = Vector2.zero;
        }

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