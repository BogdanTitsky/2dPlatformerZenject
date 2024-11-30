using System.Collections;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy.RangeAttackLogic
{
    public class EnemyRangedAttackBehaviour : EnemyAttackBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private ProjectilePool projectilePool;


        //Animator event
        public void Shoot()
        {
            Projectile projectile = CreateAndSetupProjectile();
            Vector2 direction = CalculateDirectionToTarget();
            projectile.Launch(direction);
            _currentAttackCooldown = AttackCooldown;
        }
        
        private Projectile CreateAndSetupProjectile()
        {
            Projectile projectile = projectilePool.GetProjectile();
            projectile.transform.position = firePoint.position;
            return projectile;
        }
        
        private Vector2 CalculateDirectionToTarget()
        {
            Vector2 targetPosition = _gameFactory.HeroDeathObject.transform.position;
            return (targetPosition - (Vector2)firePoint.position).normalized;
        }
        
        protected override void CheckStateExited(AnimatorState obj)
        {
        }

        protected override void StartAttack()
        {
            animator.PlayShoot();
        }

        protected override bool CanAttack() =>
            animator.State != AnimatorState.Shoot
            && CooldownIsUp() && groundChecker.IsGrounded && InRange;
    }
}