using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy.RangeAttackLogic
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] internal float Speed = 10f;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private TriggerObserver triggerObserver;
        private float _damage;
        private ProjectilePool _projectilePool;

        public abstract void Launch(Vector2 direction);
        public void Init(float damage, ProjectilePool projectilePool)
        {
            _damage = damage;
            _projectilePool = projectilePool;
            triggerObserver.TriggerEnter += OnTriggerObsEnter;
        }

        private void OnDestroy() => triggerObserver.TriggerEnter -= OnTriggerObsEnter;

        private void OnTriggerObsEnter(Collider2D obj)
        {
            if (((1 << obj.gameObject.layer) & playerLayer) != 0)
            {
                IHealth health = obj.gameObject.GetComponent<IHealth>();
                health?.TakeDamage(_damage);
            }

            _projectilePool.ReturnProjectile(this);
        }
    }
}