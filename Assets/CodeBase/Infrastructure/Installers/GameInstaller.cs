using System;
using CodeBase.Enemy.RangeAttackLogic;
using CodeBase.Environment;
using CodeBase.Infrastructure.Factory;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameFactory>().AsSingle().NonLazy();
            Container.Bind<ProjectilePool>().AsSingle();
        }
    }
}