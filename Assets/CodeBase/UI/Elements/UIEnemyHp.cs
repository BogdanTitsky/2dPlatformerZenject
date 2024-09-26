using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class UIEnemyHp : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private HpBar _hpBar;

        private IGameFactory _gameFactory;

        private void OnEnable() => 
            _health.HealthChanged += UpdateHpBar;

        private void OnDisable() => 
            _health.HealthChanged -= UpdateHpBar;

        private void UpdateHpBar() => 
            _hpBar.SetValue(_health.Current, _health.Max);
    }
}