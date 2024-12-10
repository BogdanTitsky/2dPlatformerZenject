using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy.RangeAttackLogic
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private TriggerObserver triggerObserver;
        private float _damage;
        private ProjectilePool _projectilePool;

        [Inject]
        public void Init(ProjectilePool projectilePool)
        {
            _projectilePool = projectilePool;
        }
        public virtual void Launch(Vector2 target, float speed, float damage) => _damage = damage;

        private void Start() => triggerObserver.TriggerEnter += OnTriggerObsEnter;

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