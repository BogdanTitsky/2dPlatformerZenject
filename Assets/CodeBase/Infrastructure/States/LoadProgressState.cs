using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Audio;
using CodeBase.Infrastructure.Factory;


namespace CodeBase.Infrastructure.States
{
  public class LoadProgressState : IState
  {
    private const string InitialLevel = "Main";
    private const string MenuKey = "Menu";
    private readonly IGameStateMachine _gameStateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly ISaveLoadService _saveLoadProgress;
    private readonly IAudioFactory _audioFactory;

    public LoadProgressState(IGameStateMachine gameStateMachine, IPersistentProgressService progressService, 
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
      _gameStateMachine.Enter<InitGameState>();
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