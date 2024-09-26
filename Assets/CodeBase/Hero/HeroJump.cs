using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
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

        private IInputService _input;
        private Stats _stats;

        private bool _isJumpBtnDown;
        private float _maxJumpTime = 0.2f;
        private float _currentJumpTime;
        
        private float _jumpMultiplierOnHoldingBtn = 20f;
        public float _gravityScale = 5f;

        [Inject]
        public void Init(IInputService input) => 
            _input = input;

        private void Update()
        {
            JumpInput();
        }

        private void FixedUpdate() => 
            WhileJumping();

        public void LoadProgress(PlayerProgress progress) => 
            _stats = progress.HeroStats;

        private void WhileJumping()
        {
            TryExtendJump();
            AddGravityOnFalling();
        }

        private void AddGravityOnFalling()
        {
            if (rb.velocity.y < 1f)
                rb.gravityScale = _gravityScale;
            else
                rb.gravityScale = 1;
        }

        private void TryExtendJump()
        {
            if (_isJumpBtnDown && rb.velocity.y > 0 && _currentJumpTime < _maxJumpTime)
            {
                rb.velocity += Vector2.up * (_jumpMultiplierOnHoldingBtn * Time.fixedDeltaTime);
                _currentJumpTime += Time.fixedDeltaTime;
            }
        }

        private void JumpInput()
        {
            JumpIfGrounded();

            if (_input.IsJumpButtonUp())
            {
                _isJumpBtnDown = false;
                _currentJumpTime = 0;
            } 
            
            PlayJumpAnimation();
        }

        private void JumpIfGrounded()
        {
            if (_input.IsJumpButtonDown())
            {
                Debug.Log("Jump");
            }
            if (_input.IsJumpButtonDown() && groundChecker.IsGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, _stats.JumpPower);
                _isJumpBtnDown = true;
            }
        }

        private void PlayJumpAnimation()
        {
            if (groundChecker.IsGrounded)
                heroAnimator.StopJump();
            else
                heroAnimator.PlayJump();
        }
    }
}