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
        [SerializeField] private float cooldownAfterLost = 10;
        
        public bool InSight = false;

        private IPauseService _pauseService;
        private Coroutine _aggroCoroutine;

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
            StopAggroCoroutine();
        }

        private void TriggerEnter(Collider2D obj)
        {
            InSight = true;
            StopAggroCoroutine();
        }

        private void TriggerExit(Collider2D obj)
        {
            if (gameObject.activeInHierarchy)
                _aggroCoroutine = StartCoroutine(FollowOffAfterCooldown());
        }
        
        private void StopAggroCoroutine()
        {
            if (_aggroCoroutine != null)
            {
                StopCoroutine(_aggroCoroutine);
                _aggroCoroutine = null;
            }
        }
        
        private IEnumerator FollowOffAfterCooldown()
        {
            float timePassed = 0f;
        
            while (timePassed < cooldownAfterLost)
            {
                if (!_pauseService.IsPaused)
                    timePassed += Time.deltaTime;
        
                yield return null;
            }
        
            InSight = false;
        }
    }
}
