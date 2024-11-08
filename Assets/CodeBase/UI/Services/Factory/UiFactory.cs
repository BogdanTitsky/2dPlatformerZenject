using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
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

        public void CreateWindow(WindowId windowId)
        {
            WindowConfig config = _staticData.ForWindow(windowId);
            _container.InstantiatePrefab(config.Prefab, _uiRoot);
        }
    }
}