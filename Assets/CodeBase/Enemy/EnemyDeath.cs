using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator))]
    public class EnemyDeath : MonoBehaviour
    {
        [SerializeField] private EnemyHealth health;
        [SerializeField] private EnemyAnimator animator;
        [SerializeField] private GameObject DeathFx;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private EnemyMoveToPlayer _enemyMove;
        

        public event Action OnDeath;

        private void OnEnable() => 
            health.HealthChanged += OnHealthChanged;

        private void OnDisable() => 
            health.HealthChanged -= OnHealthChanged;

        private void OnHealthChanged()
        {
            if (health.Current <= 0) 
                Die();
        }

        private void Die()
        {
            health.HealthChanged -= OnHealthChanged;
            animator.PlayDeath();
            DisableMove();
            SpawnFx();
            StartCoroutine(DestroyTimer());
            
            OnDeath?.Invoke(); 
        }

        private void DisableMove()
        {
            _enemyMove.Enabled = false;
            _rb.linearVelocity = Vector2.zero;
            _rb.isKinematic = true;
            _collider.enabled = false;
        }

        private void SpawnFx() => 
            Instantiate(DeathFx, transform.position, Quaternion.identity);

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
    }
}