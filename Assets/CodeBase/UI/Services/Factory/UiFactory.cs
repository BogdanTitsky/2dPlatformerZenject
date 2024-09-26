using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Infrastructure.Services.StaticData.Data.Windows;
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

        public UiFactory(DiContainer container, IStaticDataService staticData)
        {
            _container = container;
            _staticData = staticData;
        }

        public void CreateUiRoot() => 
            _uiRoot =  _container.InstantiatePrefabResource(AssetPath.UiRootPath).transform;

        public void CreateShop()
        {
            WindowConfig config = _staticData.ForWindow(WindowId.Shop);
            Object.Instantiate(config.Prefab, _uiRoot);
        } 
    }
}