using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private HeroAnimator heroAnimator;
        [SerializeField] private new Rigidbody2D rigidbody2D;
        [SerializeField] private float Cleavage = 1f;
        [SerializeField] private Vector2 Distance = Vector2.one;
        [SerializeField] private GroundChecker groundChecker;
        [SerializeField] private HeroMove heroMover;

        private bool IsAttackHitBox
        {
            get => _isAttackHitBox;
            set
            {
                if (_isAttackHitBox != value)
                {
                    _isAttackHitBox = value;
                    OnAttackingChanged();
                }
            } 
        }

        private bool _isAttackHitBox;
        private const string LayerName = Constants.Hittable;
        private int _layerMask;
        private IInputService _input;
        private readonly Collider2D[] _hits = new Collider2D[3];
        private Stats _stats;
        private HashSet<Collider2D> _uniqueHits = new();

        [Inject]
        public void Init(IInputService input)
        {
            _input = input;
            _layerMask = 1 << LayerMask.NameToLayer(LayerName);
        }

        private void Update()
        {
            if (IsAttackHitBox)
                ApplyAttack();

            HandleInput();
        }

        private void OnEnable()
        {
            heroAnimator.StateEntered += CheckEnteredState;
        }

        private void OnDisable()
        {
            heroAnimator.StateEntered -= CheckEnteredState;
        }

        private void CheckEnteredState(AnimatorState obj)
        {
            if (obj == AnimatorState.Attack)
                heroMover.MoveOff();
            else if (obj == AnimatorState.SecondAttack)
            {
                heroAnimator.IsAttackingOn();
                heroAnimator.OffCombo();
                heroMover.MoveOff();
            }
            else if(obj == AnimatorState.MidAirAttack)
                DisableAttackHitBox();
            else
                heroMover.MoveOn();

        }

        public void LoadProgress(PlayerProgress progress)
        {
            _stats = progress.HeroStats;
        }

        private void OnAttackingChanged()
        {
        }

        private void HandleInput()
        {
            if (_input.IsAttackButtonDown())
            {
                heroAnimator.IsAttackingOn();
            }
            if (_input.IsAttackButtonDown() && heroAnimator.State == AnimatorState.Attack) 
                heroAnimator.ContinueCombo();
            
        }

        public void EnableAttackHitBox() => 
            IsAttackHitBox = true;

        public void DisableAttackHitBox()
        {
            IsAttackHitBox = false;
            heroAnimator.IsAttackingOff();
            _uniqueHits.Clear();
        }

        private void ApplyAttack()
        {
            for (var i = 0; i < Hit(); i++)
            {
                Collider2D hit = _hits[i];
                
                if (_uniqueHits.Add(hit))
                {
                    IHealth health = hit.transform.parent.GetComponent<IHealth>();
                    if (health != null) 
                        health.TakeDamage(_stats.Damage);
                }
            }
        }

        private int Hit() => 
            Physics2D.OverlapBoxNonAlloc(StartPoint(), Size(), 0, _hits, _layerMask);


        private Vector2 StartPoint()
        {
            return Application.isPlaying
                ? new Vector2(transform.position.x, transform.position.y) +
                  _stats.AttackDistance.AsUnity2Vector() * transform.localScale
                : new Vector2(transform.position.x, transform.position.y) + Distance * transform.localScale;
        }

        private Vector2 Size()
        {
            return Application.isPlaying
                ? new Vector2(_stats.AttackCleavage, _stats.AttackCleavage)
                : new Vector2(Cleavage, Cleavage);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(StartPoint(), Size());
        }
    }
}