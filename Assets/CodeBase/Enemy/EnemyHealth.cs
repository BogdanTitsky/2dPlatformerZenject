using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private EnemyAnimator enemyAnimator;
        [SerializeField] private float _current;
        public float Max { get; set; }

        public event Action HealthChanged;

        public float Current
        {
            get => _current;
            set => _current = value;
        }

        public void TakeDamage(float damage)
        {
            if (Current <= 0)
                return;
            Current -= damage;
            if (Current > 0)
                enemyAnimator.PlayStunForSeconds(0.8f);
            HealthChanged?.Invoke();
        }

        public void Reset()
        {
            Current = Max;
            HealthChanged?.Invoke();
        }
    }
}