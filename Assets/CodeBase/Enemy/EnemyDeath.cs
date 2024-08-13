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
            
            SpawnFx();
            StartCoroutine(DestroyTimer());
            
            OnDeath?.Invoke(); 
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