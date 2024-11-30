using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Enemy.RangeAttackLogic
{
    public class ProjectilePool : MonoBehaviour
    {
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private int poolSize = 2;
        [SerializeField] private EnemyAttackBehaviour enemyAttackBehaviour;
        
        private readonly Queue<Projectile> _pool = new();

        private void Awake() => InitProjectiles();

        public Projectile GetProjectile()
        {
            Projectile projectile;
            if (_pool.Count > 0)
            {
                projectile = _pool.Dequeue();
                projectile.gameObject.SetActive(true);
                return projectile;
            }

            projectile = InstantiateProjectile();
            return projectile;
        }

        public void ReturnProjectile(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
            _pool.Enqueue(projectile);
        }

        private void InitProjectiles()
        {
            for (int i = 0; i < poolSize; i++)
            {
                Projectile projectile = InstantiateProjectile();
                projectile.gameObject.SetActive(false);
                _pool.Enqueue(projectile);
            }
        }

        private Projectile InstantiateProjectile()
        {
            Projectile projectile = Instantiate(projectilePrefab, transform);
            projectile.Init(enemyAttackBehaviour.Damage, this);
            return projectile;
        }
    }
}