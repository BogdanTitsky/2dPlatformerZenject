using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroAttack : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private HeroAnimator heroAnimator;
        [SerializeField] private Rigidbody2D rigidbody2D;
        // [SerializeField] private Vector2 _vector2;
        

        private int _layerMask;

        private IInputService _input;
        private Collider2D[] _hits  = new Collider2D[3];
        private Stats _stats;

        [Inject]
        public void Init(IInputService input)
        {
            _input = input;

            _layerMask = 1 << LayerMask.NameToLayer("Hittable");
        }

        private void Update()
        {
            if (_input.IsAttackButtonUp() && !heroAnimator.IsAttacking) heroAnimator.PlayAttack();
        }
        
        public void LoadProgress(PlayerProgress progress) => 
            _stats =  progress.HeroStats;

        public void UpdateProgress(PlayerProgress progress)
        {
            throw new System.NotImplementedException();
        }
        
        public void OnAttack()
        {
            for (int i = 0; i < Hit(); i++)
            {
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
            }
        }
        
        public void OnAttackEnd()
        {
        }
        
        private int Hit() =>
            Physics2D.OverlapBoxNonAlloc(StartPoint(),
                _stats.DamageRadius.AsUnity2Vector(),90 ,_hits, _layerMask);

        private Vector2 StartPoint() => 
            new(transform.position.x + transform.localScale.x, transform.position.y + 1f);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(StartPoint(), _stats.DamageRadius.AsUnity2Vector());
        }

       
    }
}