using System.Collections;
using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy.RangeAttackLogic
{
    public class EnemyRangedAttackBehaviour : EnemyAttackBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private float projectileSpeed = 60f;

        private ProjectilePool _projectilePool;

        [Inject]
        public void Init(ProjectilePool projectilePool) => _projectilePool = projectilePool;

        //Animator event
        public void Shoot()
        {
            Projectile projectile = _projectilePool.GetProjectile(firePoint.position);
            projectile.Launch(_gameFactory.HeroDeathObject.transform.position, projectileSpeed, Damage );
        }

        protected override void CheckStateExited(AnimatorState obj)
        {
        }
    }
}