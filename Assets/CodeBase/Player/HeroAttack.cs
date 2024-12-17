using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services.Pause;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Services.Input;
using UnityEngine;
using Zenject;

namespace CodeBase.Player
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroAttack : MonoBehaviour, ISavedProgressReader
    {
        [SerializeField] private HeroAnimator heroAnimator;
        [SerializeField] private HeroMove heroMover;
        [SerializeField] private TriggerObserver triggerObserver;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip attackClip;

        private Stats _stats;
        private readonly HashSet<Collider2D> _uniqueHits = new();

        private IInputService _input;
        private IPauseService _pauseService;

        [Inject]
        public void Init(IInputService input, IPauseService pauseService)
        {
            _pauseService = pauseService;
            _input = input;
        }

        private void OnEnable()
        {
            heroAnimator.StateEntered += CheckEnteredState;
            heroAnimator.StateExited += CheckExitedState;
            triggerObserver.TriggerEnter += OnHit;
            DisableAttackHitBox();
        }

        private void OnDisable()
        {
            heroAnimator.StateEntered -= CheckEnteredState;
            heroAnimator.StateExited -= CheckExitedState;
            triggerObserver.TriggerEnter -= OnHit;
        }

        private void Update() => HandleInput();

        public void LoadProgress(PlayerProgress progress)
        {
            _stats = progress.HeroStats;
        }

        //Animator event
        public void EnableAttackHitBox()
        {
            audioSource.PlayOneShot(attackClip);
            triggerObserver.HitBoxCollider.enabled = true;
        }

        //Animator event
        public void DisableAttackHitBox()
        {
            triggerObserver.HitBoxCollider.enabled = false;
            heroAnimator.IsAttacking(false);
            _uniqueHits.Clear();
        }

        private void CheckExitedState(AnimatorState obj)
        {
            switch (obj)
            {
                case AnimatorState.MidAirAttack:
                case AnimatorState.Attack:
                case AnimatorState.SecondAttack:
                case AnimatorState.ThirdAttack:
                    DisableAttackHitBox();
                    break;
            }
        }

        private void CheckEnteredState(AnimatorState obj) => CheckComboStates(obj);

        private void CheckComboStates(AnimatorState obj)
        {
            if (obj == AnimatorState.SecondAttack ||
                obj == AnimatorState.ThirdAttack)
            {
                heroAnimator.IsAttacking(true);
                heroAnimator.ContinueCombo(false);
            }
        }

        private void HandleInput()
        {
            if (_pauseService.IsPaused) return;
            if (_input.IsAttackButtonDown())
                heroAnimator.IsAttacking(true);

            if (_input.IsAttackButtonDown() &&
                (heroAnimator.State == AnimatorState.Attack ||
                 heroAnimator.State == AnimatorState.SecondAttack))
                 heroAnimator.ContinueCombo(true);
        }

        private void OnHit(Collider2D hit)
        {
            
            //hittable layer checks in matrix
            if (_uniqueHits.Add(hit))
            {
                IHealth health = hit.transform.parent.GetComponent<IHealth>();
                if (health != null)
                {
                    if (heroAnimator.State == AnimatorState.ThirdAttack) 
                        health.TakeDamage(_stats.Damage * 1.5f);
                    else
                        health.TakeDamage(_stats.Damage);
                }
            }
        }
    }
}