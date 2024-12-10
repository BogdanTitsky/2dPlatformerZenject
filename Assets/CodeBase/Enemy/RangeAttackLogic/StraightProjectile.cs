using UnityEngine;

namespace CodeBase.Enemy.RangeAttackLogic
{
    public class StraightProjectile : Projectile
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        private readonly Vector2 offset = new(0f, 3f);
        public override void Launch(Vector2 target, float speed, float damage)
        {
            base.Launch(target, speed, damage);
            Vector2 direction = target + offset - (Vector2)transform.position;
            _rigidbody.linearVelocity = direction.normalized * speed;
        }

        private void Update() => RotateArrow();

        private void RotateArrow()
        {
            float angle = Mathf.Atan2(_rigidbody.linearVelocity.y,_rigidbody.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    
}