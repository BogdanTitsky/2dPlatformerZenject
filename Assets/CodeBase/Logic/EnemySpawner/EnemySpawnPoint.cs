﻿using System.Threading.Tasks;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.StaticData.Data;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic.EnemySpawner
{
    public class EnemySpawnPoint : MonoBehaviour, ISavedProgress
    {
        public EnemyTypeId EnemyTypeId;
        public string Id { get;  set; }

        public bool IsSlain;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;

        [Inject]
        private void Init(IGameFactory factory)
        {
            _factory = factory;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(Id))
            {
                IsSlain = true;
                
            }
            else
                Spawn();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (IsSlain)
                progress.KillData.ClearedSpawners.Add(Id);
        }

        private async void Spawn()
        {
            GameObject enemy = await _factory.CreateEnemy(EnemyTypeId, transform);
            _enemyDeath = enemy.GetComponent<EnemyDeath>();
            _enemyDeath.OnDeath += Slay;
        }

        private void Slay()
        {
            if (_enemyDeath != null)
                _enemyDeath.OnDeath -= Slay;
            
            IsSlain = true;
        }
    }
}