using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    public abstract  class Projectile : MonoBehaviour
    {
        [SerializeField] internal float Speed = 10f;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private EnemyAttackBehaviour attack;
        
        public abstract void Launch(Vector2 direction);
        private float Damage => attack.Damage;

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (((1 << collision.gameObject.layer) & playerLayer) != 0)
            {
                IHealth health = collision.gameObject.GetComponent<IHealth>();
                health?.TakeDamage(Damage);
            }

            gameObject.SetActive(false);
        }
    }
    
    public class StraightProjectile : Projectile
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        public override void Launch(Vector2 direction)
        {
            _rigidbody.linearVelocity = direction.normalized * Speed;
        }
    }
}