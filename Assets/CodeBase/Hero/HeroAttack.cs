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
        [SerializeField] private float Cleavage = 1f;
        [SerializeField] private Vector2 Distance = Vector2.one;
        

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
            Physics2D.OverlapBoxNonAlloc(StartPoint(), Size(), 0,  _hits,_layerMask);
        
       

        private Vector2 StartPoint() =>
            Application.isPlaying 
                ? new Vector2(transform.position.x, transform.position.y) + _stats.AttackDistance.AsUnity2Vector() * transform.localScale
                : new Vector2(transform.position.x, transform.position.y) + Distance * transform.localScale;

        private Vector2 Size() => 
            Application.isPlaying 
                ? new(_stats.AttackCleavage, _stats.AttackCleavage) 
                : new(Cleavage, Cleavage);
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(StartPoint(), Size());
        }
       
    }
}