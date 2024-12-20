using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Player
{
    public class HeroJump : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private GroundChecker groundChecker;
        [SerializeField] private HeroAnimator heroAnimator;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float _jumpMultiplierOnHoldingBtn = 45f;
        [SerializeField] private float _maxJumpTime = 0.3f;

        private Stats _stats;
        private bool _isJumpBtnDown;
        private float _currentJumpTime;

        private void OnEnable() => groundChecker.GroundedChanged += OnGroundedChanged;

        private void OnDisable() => groundChecker.GroundedChanged -= OnGroundedChanged;

        private void OnGroundedChanged()
        {
            if (groundChecker.IsGrounded)
                _currentJumpTime = 0;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _stats = progress.HeroStats;
        }

        private void FixedUpdate()
        {
            JumpIfGrounded();
            TryExtendJump();
            PlayJumpAnimation();
        }

        private void JumpIfGrounded()
        {
            if (_isJumpBtnDown && groundChecker.IsGrounded 
                && heroAnimator.State != AnimatorState.Block
                && heroAnimator.State != AnimatorState.Stunned
                && heroAnimator.State != AnimatorState.Hurt)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, _stats.JumpPower);
            }
        }

        private void TryExtendJump()
        {
            if (_isJumpBtnDown && rb.linearVelocity.y > 0 && _currentJumpTime < _maxJumpTime)
            {
                rb.linearVelocity += Vector2.up * (_jumpMultiplierOnHoldingBtn * Time.deltaTime);
                _currentJumpTime += Time.deltaTime;
            }
        }

        private void PlayJumpAnimation()
        {
            if (groundChecker.IsGrounded)
                heroAnimator.StopInAir();
            else
                heroAnimator.PlayInAir();
        }
    }
}