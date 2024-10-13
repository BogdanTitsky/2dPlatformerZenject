using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.Services.StaticData.Data
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "StaticData/Enemy")]
    public class EnemyStaticData : ScriptableObject
    {
        public EnemyTypeId EnemyTypeId;

        public int MinLoot;
        public int MaxLoot;
        
        [Range(1, 100)]
        public int Hp;
        [Range(1f, 50f)]
        public float Damage;
        [Range(0.5f, 2f)]
        public float Cleavage = 1f;
        [Range(0.5f, 1f)]
        public float AttackCooldown = 1.5f;

        public Vector2 Distance = Vector2.one;
        
        public AssetReference prefabReference;
    }
}