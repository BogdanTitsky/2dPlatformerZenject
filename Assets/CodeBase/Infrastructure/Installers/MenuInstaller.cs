using CodeBase.UI.Menu;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        [SerializeField] private UIMenuRoot _uiMenuRoot;
        
        public override void InstallBindings()
        {
            Container.Bind<UIMenuRoot>().FromInstance(_uiMenuRoot).AsSingle();
        }
    }
}