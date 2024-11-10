using CodeBase.Audio;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services.Input;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameBootstrapper gameBootstrapperPrefab;
        [SerializeField] private LoadingCurtain loadingCurtain;
        [SerializeField] private AudioMixer audioMixer;

        public override void InstallBindings()
        {
            Container.Bind<ICoroutineRunner>().To<GameBootstrapper>().FromComponentInNewPrefab(gameBootstrapperPrefab)
                .AsSingle().NonLazy();
            Container.Bind<LoadingCurtain>().FromComponentInNewPrefab(loadingCurtain).AsSingle().NonLazy();
            Container.Bind<AudioMixer>().FromInstance(audioMixer).AsSingle().NonLazy();

            Container.Bind<SceneLoader>().AsSingle();
            Container.Bind<Game>().AsSingle();
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();

            RegisterServices();

            BindGameStates();
        }

        private void BindGameStates()
        {
            //Don't forget to add new states into state machine
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadProgressState>().AsSingle();
            Container.Bind<InitGameState>().AsSingle();
            Container.Bind<LoadMenuState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
            Container.Bind<ReloadLevelState>().AsSingle();
            Container.Bind<PauseGameState>().AsSingle();
        }

        private void RegisterServices()
        {
            RegisterInputService();
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle().NonLazy();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle().NonLazy();
            Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().AsSingle().NonLazy();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<IUiFactory>().To<UiFactory>().AsSingle();
            Container.Bind<IAudioFactory>().To<AudioFactory>().AsSingle();


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