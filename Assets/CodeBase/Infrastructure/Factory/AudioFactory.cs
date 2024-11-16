using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Audio;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public class AudioFactory : BaseFactory, IAudioFactory
    {
        public override List<ISavedProgressReader> ProgressReaders { get; } = new();
        public override List<ISavedProgress> ProgressWriters { get; } = new();

        private readonly BackgroundMusic _backgroundMusic;

        public AudioFactory(DiContainer container, IAssetProvider assets, IPersistentProgressService progressService, BackgroundMusic backgroundMusic) : base(container, assets, progressService)
        {
            _backgroundMusic = backgroundMusic;
        }

        public void SetupAudio()
        {
            Cleanup();
            LoadBackgroundMusic();
            InformProgressReaders();
        }

        private void LoadBackgroundMusic()
        {
            RegisterProgressWatchers(_backgroundMusic.gameObject);
        }
    }
}