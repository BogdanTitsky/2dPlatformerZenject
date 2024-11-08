using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using ModestTree;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factory
{
    public abstract class BaseFactory
    {
        public  abstract List<ISavedProgressReader> ProgressReaders { get; }
        public  abstract List<ISavedProgress> ProgressWriters { get; }
        
        private readonly DiContainer _container;
        private readonly  IAssetProvider _assets;
        private readonly  IPersistentProgressService _progress;

        protected BaseFactory(DiContainer container, IAssetProvider assets, IPersistentProgressService progressService)
        {
            _container = container;
            _assets = assets;
            _progress = progressService;
        }
        protected GameObject InstantiateRegistered(GameObject prefab, Vector3 at)
        {
            GameObject gameObject = _container.InstantiatePrefab(prefab);
            gameObject.transform.position = at;
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }
        
        protected GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = _container.InstantiatePrefab(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }
        
        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if(progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);
      
            ProgressReaders.Add(progressReader);
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
            
            _assets.Cleanup();
        }
        
        protected void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in ProgressReaders)
                progressReader.LoadProgress(_progress.Progress);
        }
    }
}