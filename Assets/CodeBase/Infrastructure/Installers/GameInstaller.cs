﻿using CodeBase.Infrastructure.Factory;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle().NonLazy();
        }
    }
}