using CodeBase.Infrastructure.Services;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadEnemies();
        EnemyStaticData ForEnemy(EnemyTypeId typeId);
    }
}