using UnityEngine;

namespace CodeBase.Enemy.RangeAttackLogic
{
    public class StraightProjectile : Projectile
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        public override void Launch(Vector2 direction)
        {
            _rigidbody.linearVelocity = direction.normalized * Speed;
        }
    }
}