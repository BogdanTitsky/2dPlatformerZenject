using UnityEngine;

namespace CodeBase.Enemy
{
    public class CheckAttackRange : MonoBehaviour
    {
        [SerializeField] private EnemyAttackBehaviour enemyAttackBehaviour;
        [SerializeField] private TriggerObserver triggerObserver;

        private void OnEnable()
        {
            triggerObserver.TriggerEnter += TriggerEnter;
            triggerObserver.TriggerExit += TriggerExit;
        }
        
        private void OnDisable()
        {
            triggerObserver.TriggerEnter -= TriggerEnter;
            triggerObserver.TriggerExit -= TriggerExit;
        }
        
        private void TriggerEnter(Collider2D obj)
        {
            enemyAttackBehaviour.InRange = true;
        }

        private void TriggerExit(Collider2D obj)
        {
            enemyAttackBehaviour.InRange = false;
        }
    }
}