using CodeBase.Audio;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.GameStates;
using CodeBase.Infrastructure.Services.Pause;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.StaticData;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Player;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private GameBootstrapper _gameBootstrapperPrefab;
        [SerializeField] private LoadingCurtain _loadingCurtain;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private BackgroundMusic _backgroundMusic;
        [SerializeField] private PlayerController _playerController;
        

        public override void InstallBindings()
        {
            Container.Bind<ICoroutineRunner>().To<GameBootstrapper>().FromComponentInNewPrefab(_gameBootstrapperPrefab)
                .AsSingle().NonLazy();
            Container.Bind<PlayerController>().FromComponentInNewPrefab(_playerController).AsSingle().NonLazy();
            Container.Bind<LoadingCurtain>().FromComponentInNewPrefab(_loadingCurtain).AsSingle().NonLazy();
            Container.Bind<AudioMixer>().FromInstance(_audioMixer).AsSingle().NonLazy();
            Container.Bind<BackgroundMusic>().FromComponentInNewPrefab(_backgroundMusic).AsSingle().NonLazy();
            Container.Bind<SceneLoader>().AsSingle();
            Container.Bind<Game>().AsSingle();
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();

            RegisterServices();

            BindGameStates();
        }

        private void BindGameStates()
        {
            //Don't forget to add new states into state machine
            Container.Bind<BootstrapGameState>().AsSingle();
            Container.Bind<LoadProgressGameState>().AsSingle();
            Container.Bind<InitGameGameState>().AsSingle();
            Container.Bind<LoadMenuGameState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<GameLoopGameState>().AsSingle();
            Container.Bind<ReloadLevelState>().AsSingle();  
        }

        private void RegisterServices()
        {
            Container.Bind<IPauseService>().To<PauseService>().AsSingle().NonLazy();
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle().NonLazy();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle().NonLazy();
            Container.Bind<IPersistentProgressService>().To<PersistentProgressService>().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().AsSingle().NonLazy();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<IUiFactory>().To<UiFactory>().AsSingle();
            Container.Bind<IAudioFactory>().To<AudioFactory>().AsSingle();
        }
    }
}