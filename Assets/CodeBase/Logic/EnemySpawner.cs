using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic
{
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        public EnemyTypeId EnemyTypeId;
        private string _id;

        public bool _IsSlain;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        [Inject]
        private void Init(IGameFactory factory)
        {
            _factory = factory;
        }

        private void Awake()
        {
            _id = GetComponent<UniqueId>().Id;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(_id))
            {
                _IsSlain = true;
            }
            else
                Spawn();
        }

        private void Spawn()
        {
            GameObject enemy = _factory.CreateEnemy(EnemyTypeId, transform);
            _enemyDeath = enemy.GetComponent<EnemyDeath>();
            _enemyDeath.OnDeath += Slay;
        }

        private void Slay()
        {
            if (_enemyDeath != null)
                _enemyDeath.OnDeath -= Slay;
            
            _IsSlain = true;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (_IsSlain)
                progress.KillData.ClearedSpawners.Add(_id);
        }
    }
}