using System.Collections;
using CodeBase.Infrastructure.Services.Pause;
using UnityEngine;
using Zenject;

namespace CodeBase.Enemy
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] private TriggerObserver triggerObserver;
        [SerializeField] private EnemyMoveToPlayer moveToPlayer;

        public bool InRange = false;

        private IPauseService _pauseService;

        [Inject]
        public void Init(IPauseService pauseService)
        {
            _pauseService = pauseService;
        }

        private void OnEnable()
        {
            triggerObserver.TriggerExit += TriggerExit;
            triggerObserver.TriggerEnter += TriggerEnter;
        }
        
        private void OnDisable()
        {
            triggerObserver.TriggerExit -= TriggerExit;
            triggerObserver.TriggerEnter -= TriggerEnter;
            // StopAggroCoroutine();
        }

        private void TriggerEnter(Collider2D obj)
        {
            InRange = true;
            // StopAggroCoroutine();
            // FollowOn();
        }

        private void TriggerExit(Collider2D obj)
        {
            InRange = false;
            //
            // if (gameObject.activeInHierarchy)
            //     _aggroCoroutine = StartCoroutine(FollowOffAfterCooldown());
        }

        // private void StopAggroCoroutine()
        // {
        //     if (_aggroCoroutine != null)
        //     {
        //         StopCoroutine(_aggroCoroutine);
        //         _aggroCoroutine = null;
        //     }
        // }
        //
        // private IEnumerator FollowOffAfterCooldown()
        // {
        //     float timePassed = 0f;
        //
        //     while (timePassed < cooldown)
        //     {
        //         if (!_pauseService.IsPaused)
        //             timePassed += Time.deltaTime;
        //
        //         yield return null;
        //     }
        //
        //     FollowOff();
        // }

        // private void FollowOn() => 
        //     moveToPlayer.enabled = true;
        //
        // private void FollowOff() => 
        //     moveToPlayer.enabled = false;
    }
}
