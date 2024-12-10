using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad
{
  public class SaveLoadService : ISaveLoadService
  {
    private const string ProgressKey = "Progress";
    
    private readonly IPersistentProgressService _progressService;

    public SaveLoadService(IPersistentProgressService progressService)
    {
      _progressService = progressService;
    }

    public void SaveProgress(List<ISavedProgress> ProgressWriters)
    {
      foreach (ISavedProgress progressWriter in ProgressWriters)
        progressWriter.UpdateProgress(_progressService.Progress);
      
      PlayerPrefs.SetString(ProgressKey, _progressService.Progress.ToJson());
    }

    public PlayerProgress LoadProgress()
    {
      return PlayerPrefs.GetString(ProgressKey)?
        .ToDeserialized<PlayerProgress>();
    }
  }
}