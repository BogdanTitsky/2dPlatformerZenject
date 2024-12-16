﻿using System;
using System.Collections;
using CodeBase.Enemy.EnemyStates;
using CodeBase.Infrastructure.StateMachine;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyStateMachine : MonoBehaviour
    {
        [SerializeField] private PlayerDetector playerDetector;
        [SerializeField] private EnemyAttackBehaviour attackBehaviour;
        [SerializeField] private EnemyMoveToPlayer moveToPlayer;
        [SerializeField] private EnemyHealth enemyHealth;
        [SerializeField] private EnemyAnimator animator;
        [SerializeField] private LootSpawner lootSpawner;
        
        private readonly StateMachine _stateMachine = new();

        #region States
        
        private EnemyWanderState wanderState;
        private EnemyChaseState chaseState;
        private EnemyAttackState attackState;
        private EnemyStunState stunState;
        private EnemyDeathState deathState;

        #endregion

        private void Start()
        {
            InitStates();
            AddTransitions();
            _stateMachine.SetState(wanderState);
        }

        private void InitStates()
        {
            wanderState = new EnemyWanderState(animator);
            chaseState = new EnemyChaseState(moveToPlayer, animator);
            attackState = new EnemyAttackState(attackBehaviour, moveToPlayer, animator);
            stunState = new EnemyStunState(animator);
            deathState = new EnemyDeathState(animator, moveToPlayer, this);
        }

        private void AddTransitions()
        {
            At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.InRange));
            At(wanderState, chaseState, new FuncPredicate(() => playerDetector.InRange));
            At(chaseState, attackState, new FuncPredicate(() => attackBehaviour.CanAttack()));
            At(attackState, chaseState, new FuncPredicate(() => !attackBehaviour.CanAttack()));
            At(stunState, wanderState, new FuncPredicate(() => stunState.StunOver()));
            Any(deathState, new FuncPredicate(() => enemyHealth.Current <= 0));
        }

        #region TakeDamageState

        private void OnEnable() => enemyHealth.HealthChanged += OnHealthChanged;

        private void OnDisable() => enemyHealth.HealthChanged -= OnHealthChanged;
        
        private void OnHealthChanged()
        {
            if (enemyHealth.Current > 0)
                _stateMachine.SetState(stunState);
        }

        #endregion

        public event Action OnDeath;
        internal void DestroyEnemy()
        {
            OnDeath?.Invoke();
            lootSpawner.SpawnLoot();
            StartCoroutine(DestroyTimer());
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
        }
        
        #region Helpers

        private void At(IState from, IState to, IPredicate condition) => 
            _stateMachine.AddTransition(from, to, condition);

        private void Any(IState to, IPredicate condition) => 
            _stateMachine.AddAnyTransition(to, condition);

        #endregion
        
        private void Update() => _stateMachine.Update();

        private void FixedUpdate() => _stateMachine.FixedUpdate();
    }
}