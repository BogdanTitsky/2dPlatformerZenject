using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.StaticData.Data;
using CodeBase.Infrastructure.Services.StaticData.Data.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string EnemiesStaticDataPath = "StaticData/Enemies";
        private const string LevelsStaticDataPath = "StaticData/Levels";
        private const string WindowStaticDataPath = "StaticData/UI/WindowStaticData";

        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<WindowId, WindowConfig> _windowConfigs;

        public void LoadStaticData()
        {
            _enemies = Resources.LoadAll<EnemyStaticData>(EnemiesStaticDataPath)
                .ToDictionary(x=> x.EnemyTypeId, x => x);
            
            _levels = Resources
                .LoadAll<LevelStaticData>(LevelsStaticDataPath)
                .ToDictionary(x=> x.LevelKey, x => x);
            
            _windowConfigs = Resources
                .Load<WindowStaticData>(WindowStaticDataPath).Configs
                .ToDictionary(x=> x.WindowId, x => x);
        }

        public EnemyStaticData ForEnemy(EnemyTypeId typeId) => 
            _enemies.TryGetValue(typeId, out EnemyStaticData staticData)
                ? staticData
                : null;

        public LevelStaticData ForLevel(string sceneKey)=> 
            _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
                ? staticData
                : null;

        public WindowConfig ForWindow(WindowId windowId) =>
            _windowConfigs.TryGetValue(windowId, out WindowConfig windowConfig)
                ? windowConfig
                : null;
    }
}