using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAttack))]
    public class CheckAttackRange : MonoBehaviour
    {
        [SerializeField] private EnemyAttack EnemyAttack;
        [SerializeField] private TriggerObserver triggerObserver;

        private void OnEnable()
        {
            triggerObserver.TriggerEnter += TriggerEnter;
            triggerObserver.TriggerExit += TriggerExit;
            
            EnemyAttack.DisableAttack();
        }
        
        private void OnDisable()
        {
            triggerObserver.TriggerEnter -= TriggerEnter;
            triggerObserver.TriggerExit -= TriggerExit;
        }
        
        private void TriggerEnter(Collider2D obj)
        {
            EnemyAttack.EnableAttack();
        }

        private void TriggerExit(Collider2D obj)
        {
            EnemyAttack.DisableAttack();
        }
    }
}