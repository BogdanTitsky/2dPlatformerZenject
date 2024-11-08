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
        public override List<ISavedProgressReader> ProgressReaders { get; }
        public override List<ISavedProgress> ProgressWriters { get; }
        
        private readonly IAssetProvider _assets;

        public AudioFactory(DiContainer container, IAssetProvider assets) : base(container, assets)
        {
            _assets = assets;
        }

        public async void SetupAudio()
        {
            await LoadBackgroundMusic();
        }

        private async Task LoadBackgroundMusic()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.BackgroundMusic);
            InstantiateRegistered(prefab);
        }
    }
}