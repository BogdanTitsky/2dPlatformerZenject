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
        //Parameters
        private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int HeroDie = Animator.StringToHash("HeroDie");

        //Animations
        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _stunStateHash = Animator.StringToHash("Stun");
        private readonly int _attackStateHash = Animator.StringToHash("Attack");
        private readonly int _shootStateHash = Animator.StringToHash("Shoot");
        private readonly int _walkingStateHash = Animator.StringToHash("Move");
        private readonly int _deathStateHash = Animator.StringToHash("Death");
        private readonly int _heroDieStateHash = Animator.StringToHash("Taunt");

        private Animator _animator;
        private IPauseService _pauseService;
        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        public AnimatorState State { get; private set; }
        private Coroutine _currentCoroutine;

        [Inject]
        private void Init(IPauseService pauseService)
        {
            _pauseService = pauseService;

            _pauseService.PauseChanged += PauseServiceOnPauseChanged;
        }

        private void PauseServiceOnPauseChanged()
        {
            _animator.speed = _pauseService.IsPaused ? 0 : 1;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnDisable()
        {
            if (_animator != null)
                _animator.Rebind();

            _pauseService.PauseChanged -= PauseServiceOnPauseChanged;
        }

        public void PlayStunForSeconds(float duration)
        {
            PlayAnimationForSeconds(duration, _stunStateHash);
        }

        public void PlayDeath()
        {
            _animator.SetTrigger(Die);
        }

        public void PlayOnHeroDie()
        {
            _animator.SetTrigger(HeroDie);
        }

        public void Move()
        {
            _animator.SetBool(IsMoving, true);
        }

        public void StopMoving()
        {
            _animator.SetBool(IsMoving, false);
        }

        public void PlayAttacking(bool value)
        {
            _animator.SetBool(IsAttacking, value);
        }

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            StateExited?.Invoke(StateFor(stateHash));
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _idleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == _attackStateHash)
                state = AnimatorState.Attack;
            else if (stateHash == _shootStateHash)
                state = AnimatorState.Shoot;
            else if (stateHash == _walkingStateHash)
                state = AnimatorState.Walking;
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

        private void PlayAnimationForSeconds(float duration, int paramHash)
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);
            _animator.CrossFade(paramHash, 0.1f);
            _currentCoroutine = StartCoroutine(AnimCoroutine(duration));
        }

        private IEnumerator AnimCoroutine(float duration)
        {
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                if (!_pauseService.IsPaused)
                    elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            _animator.CrossFade(_idleStateHash, 0.1f);
        }
    }
}