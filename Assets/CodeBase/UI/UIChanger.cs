using System;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using UnityEngine;
using Zenject;

namespace CodeBase.UI
{
    public class UIChanger : MonoBehaviour
    {
        [SerializeField] private HpBar hpBar;

        private HeroHealth _heroHealth;

        private IGameFactory _gameFactory;

        [Inject]
        public void Init(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _heroHealth = _gameFactory.HeroGameObject.GetComponent<HeroHealth>();
        }

        private void OnEnable()
        {
            _heroHealth.OnHealthChanged += UpdateHpBar;
            UpdateHpBar();
        }

        private void OnDisable() => 
            _heroHealth.OnHealthChanged -= UpdateHpBar;

        private void UpdateHpBar() => 
            hpBar.SetValue(_heroHealth.Current, _heroHealth.Max);
    }
}