using System.Collections.Generic;
using System.Threading.Tasks;
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

        private readonly IAssetProvider _assets;

        public AudioFactory(DiContainer container, IAssetProvider assets, IPersistentProgressService progressService) : base(container, assets, progressService)
        {
            _assets = assets;
        }

        public async void SetupAudio()
        {
            Cleanup();
            await LoadBackgroundMusic();
            InformProgressReaders();
        }

        private async Task LoadBackgroundMusic()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.BackgroundMusic);
            InstantiateRegistered(prefab);
        }
    }
}