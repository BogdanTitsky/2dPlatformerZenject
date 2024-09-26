using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class UIHeroHp : MonoBehaviour
    {
        [SerializeField] private HpBar hpBar;

        private IHealth _heroHealth;

        private IGameFactory _gameFactory;

        [Inject]
        public void Init(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _heroHealth = _gameFactory.HeroGameObject.GetComponent<IHealth>();
        }
        
        private void OnEnable() => 
            _heroHealth.HealthChanged += UpdateHpBar;

        private void OnDisable() => 
            _heroHealth.HealthChanged -= UpdateHpBar;

        private void UpdateHpBar() => 
            hpBar.SetValue(_heroHealth.Current, _heroHealth.Max);
    }
}