using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "GoblinData", menuName = "StaticData/GoblinData")]
    public class GoblinStaticData : ScriptableObject
    {
        public string TypeId;
        
        [Range(1, 100)]
        public int Hp;
        
        [Range(1, 999)]
        public int Damage;

        [Range(0.5f, 1)]
        public float EffectiveDistance;
        [Range(0.5f, 1)]
        public float Cleavage;

        public GameObject Prefab;
    }
}