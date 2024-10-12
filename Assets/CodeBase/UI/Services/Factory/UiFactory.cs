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

        private Transform _uiRoot;
        private Transform _uiMenuRoot;

        public UiFactory(DiContainer container, IStaticDataService staticData)
        {
            _container = container;
            _staticData = staticData;
        }

        public void CreateUiRoot() => 
            _uiRoot =  _container.InstantiatePrefabResource(AssetPath.UIRootPath).transform;

        public void CreateWindow(WindowId windowId)
        {
            WindowConfig config = _staticData.ForWindow(windowId);
            _container.InstantiatePrefab(config.Prefab, _uiRoot);
        } 
      
    }
}