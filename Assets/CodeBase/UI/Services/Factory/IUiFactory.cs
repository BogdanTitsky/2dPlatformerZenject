using System.Threading.Tasks;
using CodeBase.UI.Services.Windows;

namespace CodeBase.UI.Services.Factory
{
    public interface IUiFactory
    {
        void InitWindows();
        void ShowWindow(WindowId windowId);
        Task CreateUiRoot();
        void HideWindow(WindowId windowId);
    }
}