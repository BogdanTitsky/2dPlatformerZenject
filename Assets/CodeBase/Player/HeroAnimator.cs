using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services.Pause;
using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.Player
{
    public class HeroAnimator : MonoBehaviour, IAnimationStateReader
    {
        [SerializeField] private Animator animator;

        private const float CrossFadeDuration = 0.1f;

        //Animations
        private readonly int _idleStateHash = Animator.StringToHash("Idle");
        private readonly int _firstAttackStateHash = Animator.StringToHash("FirstAttack");
        private readonly int _secondAttackStateHash = Animator.StringToHash("SecondAttack");
        private readonly int _thirdAttackStateHash = Animator.StringToHash("ThirdAttack");
        private readonly int _walkingStateHash = Animator.StringToHash("Walking");
        private readonly int _hurtStateHash = Animator.StringToHash("Hurt");
        private readonly int _deathStateHash = Animator.StringToHash("PlayerDeath");
        private readonly int _midAirAttack = Animator.StringToHash("MidAirAttack");
        private readonly int _blockStateHash = Animator.StringToHash("Block");
        private readonly int _jumpingHash = Animator.StringToHash("Jumping");
        private readonly int _stunHash = Animator.StringToHash("Stun");

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        public AnimatorState State { get; private set; }

        private IPauseService _pauseService;

        [Inject]
        private void Init(IPauseService pauseService)
        {
            _pauseService = pauseService;

            _pauseService.PauseChanged += PauseServiceOnPauseChanged;
        }
        
        private void OnDisable()
        {
            _pauseService.PauseChanged -= PauseServiceOnPauseChanged;
        }

        
        private void PauseServiceOnPauseChanged()
        {
            animator.speed = _pauseService.IsPaused ? 0 : 1;
        }

        public void PlayHit()
        {
            animator.Play(_hurtStateHash);
        }

        public void PlayIdle() => 
            animator.CrossFade(_idleStateHash, CrossFadeDuration);

        public void PlayWalking() => 
            animator.CrossFade(_walkingStateHash, CrossFadeDuration);
        
        //ToDo cache animation
        public float CalculatePlayAttackDuration(int comboAttackNumber)
        {
            if (comboAttackNumber == 1)
            {
                animator.CrossFade(_firstAttackStateHash, CrossFadeDuration);
                return FindAnimationClip(_firstAttackStateHash);
            }
            if (comboAttackNumber == 2)
            {
                animator.CrossFade(_secondAttackStateHash, CrossFadeDuration);
                return FindAnimationClip(_secondAttackStateHash);
            }
            if (comboAttackNumber == 3)
            {
                animator.CrossFade(_thirdAttackStateHash, CrossFadeDuration);
                return FindAnimationClip(_thirdAttackStateHash);
            }
            return 0;
        }
        
        private float FindAnimationClip(int stateHash)
        {
            foreach (var clip in animator.runtimeAnimatorController.animationClips)
            {
                if (Animator.StringToHash(clip.name) == stateHash)
                {
                    return clip.length;
                }
            }
            return 0;
        }
        
        public void IsStunnedOn()
        {
        }

        public void IsStunnedOff()
        {
        }

        public void IsBlockingOn()
        {
        }

        public void IsBlockingOff()
        {
        }

        public void PlayDeath()
        {
        }

        public void PlayInAir()
        {
        }

        public void StopInAir()
        {
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
            else if (stateHash == _firstAttackStateHash)
                state = AnimatorState.Attack;
            else if (stateHash == _hurtStateHash)
                state = AnimatorState.Hurt;
            else if (stateHash == _walkingStateHash)
                state = AnimatorState.Moving;
            else if (stateHash == _deathStateHash)
                state = AnimatorState.Died;
            else if (stateHash == _midAirAttack)
                state = AnimatorState.MidAirAttack;
            else if (stateHash == _secondAttackStateHash)
                state = AnimatorState.SecondAttack;
            else if (stateHash == _thirdAttackStateHash)
                state = AnimatorState.ThirdAttack;
            else if (stateHash == _blockStateHash)
                state = AnimatorState.Block;
            else if (stateHash == _jumpingHash)
                state = AnimatorState.Jumping;
            else if (stateHash == _stunHash)
                state = AnimatorState.Stunned;
            else
                state = AnimatorState.Unknown;

            return state;
        }

        
    }
}