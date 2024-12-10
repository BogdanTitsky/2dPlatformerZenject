using System.Collections.Generic;
using CodeBase.Infrastructure.Services.PersistentProgress;

namespace CodeBase.Infrastructure.Factory
{
    public interface IAudioFactory
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        
        void SetupAudio();
    }
}