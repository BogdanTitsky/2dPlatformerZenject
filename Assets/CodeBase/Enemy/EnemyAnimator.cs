using System;
using CodeBase.Infrastructure.Services.Pause;
using CodeBase.Logic;
using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
  public class EnemyAnimator : MonoBehaviour, IAnimationStateReader
  {
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int HeroDie = Animator.StringToHash("HeroDie");

    private readonly int _idleStateHash = Animator.StringToHash("idle");
    private readonly int _attackStateHash = Animator.StringToHash("Attack");
    private readonly int _walkingStateHash = Animator.StringToHash("Move");
    private readonly int _deathStateHash = Animator.StringToHash("die");
    private readonly int _heroDieStateHash = Animator.StringToHash("Taunt");

    private Animator _animator;
    private IPauseService _pauseService;
    public event Action<AnimatorState> StateEntered;
    public event Action<AnimatorState> StateExited;

    public AnimatorState State { get; private set; }
    
    [Inject]
    private void Init(IPauseService pauseService)
    {
      _pauseService = pauseService;
      
      _pauseService.PauseChanged += PauseServiceOnPauseChanged;
    }

    private void PauseServiceOnPauseChanged() => 
      _animator.speed = _pauseService.IsPaused ? 0 : 1;

    private void Awake() => 
      _animator = GetComponent<Animator>();
    
  private void OnDisable()
  {
    if (_animator != null)
      _animator.Rebind(); 
    
    _pauseService.PauseChanged -= PauseServiceOnPauseChanged;
  }
    public void PlayHit() => _animator.SetTrigger(Hit);
    public void PlayDeath() => _animator.SetTrigger(Die);
    
    public void PlayOnHeroDie() => _animator.SetTrigger(HeroDie);

    public void Move()
    {
      _animator.SetBool(IsMoving, true);
      // _animator.SetFloat(Speed, speed);
    }

    public void StopMoving() => _animator.SetBool(IsMoving, false);

    public void PlayAttack() => _animator.SetTrigger(Attack);

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
      else if (stateHash == _walkingStateHash)
        state = AnimatorState.Walking;
      else if (stateHash == _deathStateHash)
        state = AnimatorState.Died;
      else if (stateHash == _deathStateHash)
        state = AnimatorState.Taunt;
      else
        state = AnimatorState.Unknown;
      
      return state;
    }
  }
}