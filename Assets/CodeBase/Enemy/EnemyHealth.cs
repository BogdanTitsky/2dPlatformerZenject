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
        [SerializeField] private GameObject _deathFx;
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
            {
                Instantiate(_deathFx, transform.position, Quaternion.identity);
                return;
            }

            Current -= damage;
            HealthChanged?.Invoke();
        }

        public void Reset()
        {
            Current = Max;
            HealthChanged?.Invoke();
        }
    }
}