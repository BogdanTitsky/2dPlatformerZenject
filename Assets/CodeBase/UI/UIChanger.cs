using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.UI
{
    public class UIChanger : MonoBehaviour
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
        
        private void Start()
        {
            IHealth enemyHealth = GetComponent<IHealth>();
            if (enemyHealth != null)
            {
                _heroHealth = enemyHealth;
            }
            
            _heroHealth.HealthChanged += UpdateHpBar;

        }

        private void OnDisable() => 
            _heroHealth.HealthChanged -= UpdateHpBar;

        private void UpdateHpBar() => 
            hpBar.SetValue(_heroHealth.Current, _heroHealth.Max);
    }
}