using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services.Input;
using CodeBase.Services.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameBootstrapper _gameBootstrapperPrefab;
        [SerializeField] private LoadingCurtain _loadingCurtain;

        public override void InstallBindings()
        {
            Container.Bind<ICoroutineRunner>().To<GameBootstrapper>().FromComponentInNewPrefab(_gameBootstrapperPrefab)
                .AsSingle().NonLazy();
            Container.Bind<LoadingCurtain>().FromComponentInNewPrefab(_loadingCurtain).AsSingle();

            Container.Bind<SceneLoader>().AsSingle();
            Container.Bind<Game>().AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();

            RegisterServices();

            BindGameStates();
        }

        private void BindGameStates()
        {
            //Don't forget to add new states into state machine
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<LoadProgressState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
        }

        private void RegisterServices()
        {
            RegisterInputService();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle();
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle().NonLazy();
            

            void RegisterInputService()
            {
                if (Application.isEditor)
                    Container.Bind<IInputService>().To<StandaloneInputService>().AsSingle();
                else
                    Container.Bind<IInputService>().To<MobileInputService>().AsSingle();
            }
        }
    }
}