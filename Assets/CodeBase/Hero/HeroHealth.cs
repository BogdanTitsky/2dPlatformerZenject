using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
    {
        [SerializeField] private HeroAnimator animator;
        private float _current;
        private Stats _stats;
        
        public event Action HealthChanged;

        public float Current
        {
            get => _current;
            set
            {
                if (_current != value)
                {
                    _current = value;
                    HealthChanged?.Invoke();
                }
            }
        }

        public float Max
        {
            get => _stats.MaxHp;
            set => _stats.MaxHp = value;
        }


        public void LoadProgress(PlayerProgress progress)
        {
            _stats = progress.HeroStats;
            Current = _stats.CurrentHp;
            HealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroStats.CurrentHp = Current;
            progress.HeroStats.MaxHp = Max;
        }

        public void TakeDamage(float damage)
        {
            if (Current <= 0)
                return;
            Current -= damage;
            if (Current > 0)
                animator.PlayHit();
        }
    }
}