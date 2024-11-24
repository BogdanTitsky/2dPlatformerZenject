using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.Pause;
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
        private IPauseService _pauseService;
        private Stats _stats;
        private bool _isJumpBtnDown;
        private float _currentJumpTime;


        [Inject]
        public void Init(IInputService input, IPauseService pauseService)
        {
            _pauseService = pauseService;
            _input = input;
        }

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

        private void Update()
        {
            JumpInput();
        }

        private void FixedUpdate()
        {
            JumpIfGrounded();
            TryExtendJump();
            PlayJumpAnimation();
        }

        private void JumpInput()
        {
            if (_pauseService.IsPaused)
            {
                JumpButtonUp();
                return;
            }

            if (_input.IsJumpButtonUp())
                JumpButtonUp();
            else if (_input.IsJumpButtonDown())
                _isJumpBtnDown = true;
        }

        private void JumpButtonUp()
        {
            _isJumpBtnDown = false;
        }

        private void JumpIfGrounded()
        {
            if (_isJumpBtnDown && groundChecker.IsGrounded 
                && heroAnimator.State != AnimatorState.Block
                && heroAnimator.State != AnimatorState.Stunned
                && heroAnimator.State != AnimatorState.Hurt)
            {
                heroAnimator.PlayJump();
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