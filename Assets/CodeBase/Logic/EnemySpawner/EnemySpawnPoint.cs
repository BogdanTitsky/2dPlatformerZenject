﻿using CodeBase.Data;
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
        private EnemyStateMachine _enemyStateMachine;
        private GameObject _enemyObject;
        
        [Inject]
        private void Init(IGameFactory factory)
        {
            _factory = factory;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(Id))
                IsSlain = true;
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
            if (_enemyObject == null)
            {
                _enemyObject = await _factory.CreateEnemy(EnemyTypeId, transform);
                _enemyStateMachine = _enemyObject.GetComponent<EnemyStateMachine>();
                _enemyStateMachine.OnDeath += Slay;
            }
            else
            {
                _enemyObject.transform.position = transform.position;
                _enemyObject.GetComponent<EnemyHealth>().Reset();
            }
        }

        private void Slay() 
        {
            if (_enemyStateMachine != null)
                _enemyStateMachine.OnDeath -= Slay;
            
            IsSlain = true;
        }
    }
}