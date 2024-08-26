using System;
using CodeBase.Logic;
using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
  public class HeroAnimator : MonoBehaviour, IAnimationStateReader
  {
    [SerializeField] public Animator _animator;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DieHash = Animator.StringToHash("Die");

    private readonly int _idleStateHash = Animator.StringToHash("Idle");
    private readonly int _idleStateFullHash = Animator.StringToHash("Base Layer.Idle");
    private readonly int _attackStateHash = Animator.StringToHash("Attack");
    private readonly int _walkingStateHash = Animator.StringToHash("Run");
    private readonly int _deathStateHash = Animator.StringToHash("Die");

    public event Action<AnimatorState> StateEntered;
    public event Action<AnimatorState> StateExited;

    public AnimatorState State { get; private set; }
    public bool IsAttacking => State == AnimatorState.Attack;

    private IInputService _inputService;

    [Inject]
    private void Init(IInputService inputService)
    {
      _inputService = inputService;
    }
    private void Update()
    {
      _animator.SetFloat(Speed, Math.Abs(_inputService.Axis.x), 0.1f, Time.deltaTime);
      _animator.SetBool(IsMovingHash, Math.Abs(_inputService.Axis.x) > 0);
    }

    public void PlayHit()
    {
      _animator.SetTrigger(HitHash);
    }

    public void PlayAttack()
    {
      _animator.SetTrigger(AttackHash);
    }

    public void PlayDeath()
    {
      _animator.SetTrigger(DieHash);
    }

    public void PlayJump() => 
      _animator.SetBool(IsJumpingHash, true);

    public void StopJump() => 
      _animator.SetBool(IsJumpingHash, false);

    public void ResetToIdle()
    {
      _animator.Play(_idleStateHash, -1);
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
      {
        state = AnimatorState.Idle;
      }
      else if (stateHash == _attackStateHash)
      {
        state = AnimatorState.Attack;
      }
      else if (stateHash == _walkingStateHash)
      {
        state = AnimatorState.Walking;
      }
      else if (stateHash == _deathStateHash)
      {
        state = AnimatorState.Died;
      }
      else
      {
        state = AnimatorState.Unknown;
      }

      return state;
    }
  }
}