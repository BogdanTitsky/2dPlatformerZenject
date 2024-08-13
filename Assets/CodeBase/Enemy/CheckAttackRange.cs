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
            triggerObserver.TriggerExit += OnTriggerExit;
            
            attack.DisableAttack();
        }
        
        private void OnDisable()
        {
            triggerObserver.TriggerEnter -= TriggerEnter;
            triggerObserver.TriggerExit -= OnTriggerExit;
        }
        
        private void TriggerEnter(Collider2D obj)
        {
            attack.EnableAttack();
        }

        private void OnTriggerExit(Collider2D obj)
        {
            attack.DisableAttack();

        }
    }
}