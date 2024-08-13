using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private HeroAnimator animator;
        private State _state;

        public event Action OnHealthChanged;

        public float Current
        {
            get => _state.CurrentHp;
            set
            {
                if (_state.CurrentHp == value)
                    return;
                _state.CurrentHp = value;
                OnHealthChanged?.Invoke();
            }
        }

        public float Max
        {
            get => _state.MaxHp;
            set => _state.MaxHp = value;
        }


        public void LoadProgress(PlayerProgress progress)
        {
            _state = progress.HeroState;
            OnHealthChanged?.Invoke();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.HeroState.CurrentHp = Current;
            progress.HeroState.MaxHp = Max;
        }

        public void TakeDamage(float damage)
        {
            if (Current <= 0)
            {
                // Hero died
                // TODO: Handle death animation and event
                return;
            }

            Current -= damage;
            animator.PlayHit();
        }
    }
}