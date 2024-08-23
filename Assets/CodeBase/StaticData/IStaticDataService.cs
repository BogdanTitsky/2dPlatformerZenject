﻿using CodeBase.Infrastructure.Services;

namespace CodeBase.StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadEnemies();
        EnemyStaticData ForEnemy(EnemyTypeId typeId);
        LevelStaticData ForLevel(string sceneKey);
    }
}