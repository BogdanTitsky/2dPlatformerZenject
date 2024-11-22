﻿using System;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private HeroHealth health;
        [SerializeField] private HeroMove heroMove;
        [SerializeField] private HeroAnimator heroAnimator;
        [SerializeField] private HeroAttack attack;

        public event Action OnHeroDeath;
        public GameObject DeathFx;
        private bool _isDead;

        private IWindowService _windowService;
    
        [Inject]
        public void Init(IWindowService windowService)
        {
            _windowService = windowService;
        }
        
        private void OnEnable()
        {
            health.HealthChanged += HealthChanged;
        }

        private void OnDisable()
        {
            health.HealthChanged -= HealthChanged;
        }

        private void HealthChanged()
        {
            if(!_isDead && health.Current <= 0)
                Die();
        }

        private void Die()
        {
            OnHeroDeath?.Invoke();
            _isDead = true;
            attack.enabled = false;
            heroAnimator.PlayDeath();
            
            Instantiate(DeathFx, transform.position, Quaternion.identity);
        }

        private void ShowGameEndPopUp()
        {
            _windowService.Open(WindowId.Lose);
        }
    }
}