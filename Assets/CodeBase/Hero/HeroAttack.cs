using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.Pause;
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
        [SerializeField] private float Cleavage = 1f;
        [SerializeField] private Vector2 Distance = Vector2.one;
        [SerializeField] private HeroMove heroMover;
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private AudioClip attackClip;
        
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
        private readonly Collider2D[] _hits = new Collider2D[3];
        private Stats _stats;
        private HashSet<Collider2D> _uniqueHits = new();
        ContactFilter2D contactFilter;
        private IInputService _input;
        private IPauseService _pauseService;

        [Inject]
        public void Init(IInputService input, IPauseService pauseService)
        {
            _pauseService = pauseService;
            _input = input;
            
            contactFilter.SetLayerMask(1 << LayerMask.NameToLayer(LayerName));
            contactFilter.useTriggers = true;
        }
        private void OnEnable()
        {
            heroAnimator.StateEntered += CheckEnteredState;
            heroAnimator.StateExited += CheckExitedState;
        }

        private void OnDisable()
        {
            heroAnimator.StateEntered -= CheckEnteredState;
            heroAnimator.StateExited -= CheckExitedState;
        }
        private void Update()
        {           
            HandleInput();
        }

        private void FixedUpdate()
        {
            if (IsAttackHitBox && !_pauseService.IsPaused)
                ApplyAttack();
        }

        private void CheckExitedState(AnimatorState obj)
        {
            switch (obj)
            {
                case AnimatorState.MidAirAttack:
                case AnimatorState.Attack:
                case AnimatorState.SecondAttack:
                    DisableAttackHitBox();
                    break;
            }
        }

        private void CheckEnteredState(AnimatorState obj)
        {
            if (obj == AnimatorState.Attack)
            {
                heroMover.MoveOff();
            }
            else if (obj == AnimatorState.SecondAttack)
            {
                heroAnimator.IsAttackingOn();
                heroAnimator.OffCombo();
                heroMover.MoveOff();
            }
            else
                heroMover.MoveOn();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _stats = progress.HeroStats;
            Distance = _stats.AttackDistance.AsUnity2Vector();
            Cleavage = _stats.AttackCleavage;
        }

        private void OnAttackingChanged()
        {
           
        }

        private void HandleInput()
        {
            if (_input.IsAttackButtonDown()) 
                heroAnimator.IsAttackingOn(); 
            
            if (_input.IsAttackButtonDown() && heroAnimator.State == AnimatorState.Attack) 
                heroAnimator.ContinueCombo();
        }

        public void EnableAttackHitBox()
        {
            audioSource.PlayOneShot(attackClip);
            IsAttackHitBox = true;
        }

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
            Physics2D.OverlapBox(StartPoint(), Size(), 0,contactFilter , _hits);

        private Vector2 StartPoint() =>
            new Vector2(transform.position.x, transform.position.y) + 
            Distance * transform.localScale;

        private Vector2 Size() => 
            new(Cleavage, Cleavage);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(StartPoint(), Size());
        }
    }
}