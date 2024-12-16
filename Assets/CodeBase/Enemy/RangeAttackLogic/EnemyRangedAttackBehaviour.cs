using System;
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

        public override bool CanAttack()
        {
            Vector2 directionToPlayer =
                (_gameFactory.HeroDeathObject.transform.position - firePoint.position).normalized;
            float distanceToPlayer =
                Vector2.Distance(firePoint.position, _gameFactory.HeroDeathObject.transform.position);

            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, directionToPlayer, distanceToPlayer, LayerMask.GetMask("Ground"));
            
            return !hit.collider && base.CanAttack();
        }

        protected override void CheckStateExited(AnimatorState obj)
        {
        }
    }
}