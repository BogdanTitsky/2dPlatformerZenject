﻿using System;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.StaticData.Data
{
    [Serializable]
    public class EnemySpawnerData
    {
        public string Id;
        public EnemyTypeId EnemyTypeId;
        public Vector3 Position;

        public EnemySpawnerData(string id, EnemyTypeId enemyTypeId, Vector3 position)
        {
            Id = id;
            EnemyTypeId = enemyTypeId;
            Position = position;
        }
    }
}