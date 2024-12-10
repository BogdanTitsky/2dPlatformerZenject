using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Infrastructure.Services.StaticData.Data.Windows;
using CodeBase.UI.Menu;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Services.Factory
{
    public class UiFactory : IUiFactory
    {
        private readonly DiContainer _container;
        private readonly IStaticDataService _staticData;
        private readonly IAssetProvider _assets;

        private Transform _uiRoot;
        private Transform _uiMenuRoot;

        Dictionary<WindowId, GameObject> _windowDictionary = new();
        public UiFactory(DiContainer container, IStaticDataService staticData, IAssetProvider assets)
        {
            _container = container;
            _staticData = staticData;
            _assets = assets;
            
        }

        public async Task CreateUiRoot()
        {
            GameObject prefab =  await _assets.Load<GameObject>(AssetAddress.UIRoot);
            _uiRoot = _container.InstantiatePrefab(prefab).transform;
        }

        public void InitWindows()
        {
            foreach (KeyValuePair<WindowId, WindowConfig> window in _staticData.GetWindowConfigs())
            {
                GameObject windowInstance =  _container.InstantiatePrefab(window.Value.Prefab, _uiRoot);
                _windowDictionary.Add(window.Key, windowInstance);
                windowInstance.SetActive(false);
            }
        }

        public void ShowWindow(WindowId windowId)
        {
            if (_windowDictionary.TryGetValue(windowId, out GameObject windowInstance)) 
                windowInstance.SetActive(true);
        }

        public void HideWindow(WindowId windowId)
        {
            if (_windowDictionary.TryGetValue(windowId, out GameObject windowInstance)) 
                windowInstance.SetActive(false);
        }
    }
}