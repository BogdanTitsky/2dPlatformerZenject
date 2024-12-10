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
        [Range(0.5f, 1f)]
        public float AttackCooldown = 1f;

        public AssetReference prefabReference;
    }
}