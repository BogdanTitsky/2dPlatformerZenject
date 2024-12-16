using CodeBase.Infrastructure.StateMachine;
using CodeBase.Logic;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy.EnemyStates
{
    public class EnemyWanderState : EnemyBaseState
    {
        public EnemyWanderState(EnemyAnimator animator) : base(animator)
        {
        }

        public override void OnEnter()
        {            Debug.Log("WanderState");

            animator.PlayIdle();
        }

        public override void Update()
        {
        }
    }
}