using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy.RangeAttackLogic
{
    public class ProjectilePool
    {
        private readonly Queue<Projectile> _pool = new();
        private IGameFactory _factory;

        [Inject]
        public ProjectilePool( IGameFactory factory)
        {
            _factory = factory;
        }
        
        public Projectile GetProjectile(Vector3 initPos)
        {
            Projectile projectile;
            if (_pool.Count > 0)
                projectile = _pool.Dequeue();
            else
                projectile = _factory.CreateProjectile();

            projectile.transform.position = initPos;
            projectile.gameObject.SetActive(true);
            return projectile;
        }

        public void ReturnProjectile(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
            _pool.Enqueue(projectile);
        }
    }
}