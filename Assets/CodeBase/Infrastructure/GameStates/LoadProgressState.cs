using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.States;

namespace CodeBase.Infrastructure.GameStates
{
  public class LoadProgressGameState : IGameState
  {
    private const string InitialLevel = "Level1";
    private readonly IGameStateMachine _gameStateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadProgress;
    private readonly IAudioFactory _audioFactory;

    public LoadProgressGameState(IGameStateMachine gameStateMachine, IPersistentProgressService progressService, 
      ISaveLoadService saveLoadProgress, IAudioFactory audioFactory)
    {
      _gameStateMachine = gameStateMachine;
      _progressService = progressService;
      _saveLoadProgress = saveLoadProgress;
      _audioFactory = audioFactory;
    }

    public void Enter()
    {
      LoadProgressOrInitNew();
      _gameStateMachine.Enter<InitGameGameState>();
    }

    public void Exit()
    {
    }

    private void LoadProgressOrInitNew()
    {
      _progressService.Progress = 
        _saveLoadProgress.LoadProgress() 
        ?? NewProgress();
    }

    private PlayerProgress NewProgress()
    {
      PlayerProgress progress = new(InitialLevel);

      progress.HeroStats.ResetHp();
      return progress;
    }
  }
}