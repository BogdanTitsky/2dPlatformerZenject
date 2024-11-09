using System;
using CodeBase.UI.Services.Factory;

namespace CodeBase.UI.Services.Windows
{
    public class WindowService : IWindowService
    {
        private IUiFactory _uiFactory;

        public WindowService(IUiFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }
        public void Open(WindowId windowId)
        {
            _uiFactory.CreateWindow(windowId);
        }
    }
}