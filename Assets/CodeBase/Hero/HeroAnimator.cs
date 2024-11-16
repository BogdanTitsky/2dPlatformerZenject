using System;
using CodeBase.Infrastructure.Services.Pause;
using CodeBase.Logic;
using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
  public class HeroAnimator : MonoBehaviour, IAnimationStateReader
  {
    [SerializeField] public Animator animator;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int InAirHash = Animator.StringToHash("InAir");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");
    private static readonly int IsComboContinueHash = Animator.StringToHash("IsComboContinue");
    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DieHash = Animator.StringToHash("Die");

    private readonly int _idleStateHash = Animator.StringToHash("Idle");
    private readonly int _attackStateHash = Animator.StringToHash("Attack");
    private readonly int _secondAttackStateHash = Animator.StringToHash("SecondAttack");
    private readonly int _walkingStateHash = Animator.StringToHash("Run");
    private readonly int _deathStateHash = Animator.StringToHash("Die");
    private readonly int _midAirAttack = Animator.StringToHash("MidAirAttack");

    public event Action<AnimatorState> StateEntered;
    public event Action<AnimatorState> StateExited;

    public AnimatorState State { get; private set; }

    private IInputService _inputService;
    private IPauseService _pauseService;

    [Inject]
    private void Init(IInputService inputService, IPauseService pauseService)
    {
      _pauseService = pauseService;
      _inputService = inputService;
      
      _pauseService.PauseChanged += PauseServiceOnPauseChanged;
    }

    private void OnDisable()
    {
      _pauseService.PauseChanged -= PauseServiceOnPauseChanged;
    }

    private void PauseServiceOnPauseChanged() => 
      animator.speed = _pauseService.IsPaused ? 0 : 1;

    private void Update()
    {
      if (_pauseService.IsPaused)
        return;

      animator.SetFloat(Speed, Math.Abs(_inputService.Axis.x), 0.1f, Time.deltaTime);
      animator.SetBool(IsMovingHash, Math.Abs(_inputService.Axis.x) > 0);
    }

    public void PlayHit()
    {
      animator.SetTrigger(HitHash);
    }

    public void IsAttackingOn() => 
      animator.SetBool(IsAttackingHash, true);

    public void IsAttackingOff() => 
      animator.SetBool(IsAttackingHash, false);

    public void ContinueCombo() => 
      animator.SetBool(IsComboContinueHash, true);
    
    public void OffCombo() => 
      animator.SetBool(IsComboContinueHash, false);

    public void PlayDeath()
    {
      animator.SetTrigger(DieHash);
    }

    public void PlayInAir() => 
      animator.SetBool(InAirHash, true);
    
    public void PlayJump() => 
      animator.SetTrigger(JumpHash);

    public void StopInAir() => 
      animator.SetBool(InAirHash, false);

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
      else if (stateHash == _midAirAttack)
      {
        state = AnimatorState.MidAirAttack;
      }
      else if (stateHash == _secondAttackStateHash)
      {
        state = AnimatorState.SecondAttack;
      }
      else
      {
        state = AnimatorState.Unknown;
      }

      return state;
    }
  }
}