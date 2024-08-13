using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Attack))]
    public class CheckAttackRange : MonoBehaviour
    {
        [SerializeField] private Attack attack;
        [SerializeField] private TriggerObserver triggerObserver;

        private void OnEnable()
        {
            triggerObserver.TriggerEnter += TriggerEnter;
            triggerObserver.TriggerExit += TriggerExit;
            
            attack.DisableAttack();
        }
        
        private void OnDisable()
        {
            triggerObserver.TriggerEnter -= TriggerEnter;
            triggerObserver.TriggerExit -= TriggerExit;
        }
        
        private void TriggerEnter(Collider2D obj)
        {
            attack.EnableAttack();
        }

        private void TriggerExit(Collider2D obj)
        {
            attack.DisableAttack();
        }
    }
}