using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    public class HeroJump : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private GroundChecker groundChecker;
        [SerializeField] private HeroAnimator heroAnimator;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float _jumpMultiplierOnHoldingBtn = 45f;
        [SerializeField] private float _maxJumpTime = 0.3f;

        private IInputService _input;
        private Stats _stats;
        private bool _isJumpBtnDown;
        private float _currentJumpTime;


        [Inject]
        public void Init(IInputService input) => 
            _input = input;

        public void LoadProgress(PlayerProgress progress) => 
            _stats = progress.HeroStats;

        private void FixedUpdate() => 
            WhileJumping();

        private void Update() => JumpInput();

        private void JumpInput()
        {
            JumpIfGrounded();

            if (_input.IsJumpButtonUp())
            {
                _isJumpBtnDown = false;
                _currentJumpTime = 0;
            } 
            else if (_input.IsJumpButtonDown()) 
                _isJumpBtnDown = true;
            
            PlayJumpAnimation();
        }

        private void WhileJumping()
        {
            TryExtendJump();
        }

        private void JumpIfGrounded()
        {
            if (_isJumpBtnDown && groundChecker.IsGrounded)
            {
                heroAnimator.PlayJump();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, _stats.JumpPower);
            }
        }

        private void TryExtendJump()
        {
            if (_isJumpBtnDown && rb.linearVelocity.y > 0 && _currentJumpTime < _maxJumpTime )
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