using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class Aggro : MonoBehaviour
    {
        [SerializeField] private TriggerObserver triggerObserver;
        [SerializeField] private EnemyMoveToPlayer moveToPlayer;
        [SerializeField] private float cooldown = 2;
        private Coroutine _aggroCoroutine;

        private bool _hasAggroTarget;
        
        private void Start()
        {
            triggerObserver.TriggerExit += TriggerExit;
            triggerObserver.TriggerEnter += TriggerEnter;
            FollowOff();
        }

        private void TriggerEnter(Collider2D obj)
        {
            if (_hasAggroTarget)
                return;
            _hasAggroTarget = true;
            StopAggroCoroutine();
            FollowOn();
        }

        private void TriggerExit(Collider2D obj)
        {
            if (!_hasAggroTarget)
                return;
            _hasAggroTarget = false;
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
            yield return new WaitForSeconds(cooldown);
            FollowOff();
        }

        private void FollowOn() => 
            moveToPlayer.enabled = true;
        
        private void FollowOff() => 
            moveToPlayer.enabled = false;
    }
}
