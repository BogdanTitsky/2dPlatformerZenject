using System.Collections.Generic;
using CodeBase.Infrastructure.Services.StaticData.Data;
using CodeBase.Infrastructure.Services.StaticData.Data.Windows;
using CodeBase.UI.Services.Windows;

namespace CodeBase.Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadStaticData();
        EnemyStaticData ForEnemy(EnemyTypeId typeId);
        LevelStaticData ForLevel(string sceneKey);
        WindowConfig ForWindow(WindowId windowId);
        Dictionary<WindowId, WindowConfig> GetWindowConfigs();
    }
}