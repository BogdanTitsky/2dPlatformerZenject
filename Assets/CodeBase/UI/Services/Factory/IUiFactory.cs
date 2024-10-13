using System.Threading.Tasks;
using CodeBase.UI.Services.Windows;

namespace CodeBase.UI.Services.Factory
{
    public interface IUiFactory
    {
        void CreateWindow(WindowId windowId);
        Task CreateUiRoot();
    }
}