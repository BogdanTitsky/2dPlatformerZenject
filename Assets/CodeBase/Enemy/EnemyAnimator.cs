using System;
using System.Collections;
using CodeBase.Infrastructure.Services.Pause;
using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    public class EnemyAnimator : MonoBehaviour, IAnimationStateReader
    {
        //Animations
        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _stunStateHash = Animator.StringToHash("Stun");
        private readonly int _attackStateHash = Animator.StringToHash("Attack");
        private readonly int _shootStateHash = Animator.StringToHash("Shoot");
        private readonly int _moveStateHash = Animator.StringToHash("Move");
        private readonly int _deathStateHash = Animator.StringToHash("Death");
        private readonly int _heroDieStateHash = Animator.StringToHash("Taunt");

        private Animator _animator;
        private IPauseService _pauseService;
        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        public AnimatorState State { get; private set; }
        private Coroutine _currentCoroutine;
        private const float CrossFadeDuration = 0.1f;

        [Inject]
        private void Init(IPauseService pauseService)
        {
            _pauseService = pauseService;
            _pauseService.PauseChanged += PauseServiceOnPauseChanged;
        }

        private void PauseServiceOnPauseChanged() => 
            _animator.speed = _pauseService.IsPaused ? 0 : 1;

        private void Awake() => _animator = GetComponent<Animator>();

        private void OnDisable()
        {
            if (_animator != null)
                _animator.Rebind();

            _pauseService.PauseChanged -= PauseServiceOnPauseChanged;
        }

        public void PlayStun() => _animator.CrossFade(_stunStateHash, CrossFadeDuration);

        public void PlayDeath() => _animator.CrossFade(_deathStateHash, CrossFadeDuration);
        
        public void PlayIdle() => _animator.CrossFade(_idleStateHash, CrossFadeDuration);

        public void PlayMove() => _animator.CrossFade(_moveStateHash, CrossFadeDuration);

        public void Attack() => _animator.CrossFade(_attackStateHash, CrossFadeDuration);

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash) => 
            StateExited?.Invoke(StateFor(stateHash));

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _idleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == _attackStateHash)
                state = AnimatorState.Attack;
            else if (stateHash == _shootStateHash)
                state = AnimatorState.Shoot;
            else if (stateHash == _moveStateHash)
                state = AnimatorState.Moving;
            else if (stateHash == _deathStateHash)
                state = AnimatorState.Died;
            else if (stateHash == _heroDieStateHash)
                state = AnimatorState.Taunt;
            else if (stateHash == _stunStateHash)
                state = AnimatorState.Stunned;
            else
                state = AnimatorState.Unknown;
            return state;
        }
    }
}