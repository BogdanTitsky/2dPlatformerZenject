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
        [SerializeField] private HeroBlock heroBlock;
        
        private Stats _stats;
        public event Action HealthChanged;

        private float _current;
        
        private bool IsBlocking => heroBlock.IsBlockBtnDown;

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
            if (IsBlocking)
            {
                heroBlock.BlockDamage(damage);
                return;
            }
            if (Current <= 0)
                return;
            Current -= damage;
            if (Current > 0)
                animator.PlayHit();
        }
    }
}