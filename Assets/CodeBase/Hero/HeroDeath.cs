using UnityEngine;

namespace CodeBase.Hero
{
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private HeroHealth health;
        [SerializeField] private HeroMove heroMove;
        [SerializeField] private HeroAnimator heroAnimator;
        [SerializeField] private HeroAttack attack;

        public GameObject DeathFx;
        private bool _isDead;

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
            _isDead = true;
            heroMove.enabled = false;
            attack.enabled = false;
            heroMove.MoveOff();
            heroAnimator.PlayDeath();
            
            Instantiate(DeathFx, transform.position, Quaternion.identity);
        }
    }
}