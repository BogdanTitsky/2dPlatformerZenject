using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services.Pause;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
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

        private void OnEnable()
        {
            triggerObserver.TriggerEnter += OnHit;
            DisableAttackHitBox();
        }

        private void OnDisable()
        {
            triggerObserver.TriggerEnter -= OnHit;
        }

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
            _uniqueHits.Clear();
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