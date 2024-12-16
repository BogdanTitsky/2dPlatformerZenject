using CodeBase.Infrastructure.StateMachine;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy.EnemyStates
{
    public abstract class EnemyBaseState : IState
    {
        protected readonly EnemyAnimator animator;

        protected EnemyBaseState(EnemyAnimator animator)
        {
            this.animator = animator;
        }

        public virtual void OnEnter()
        {
            // noop
        }

        public virtual void Update()
        {
            // noop
        }

        public virtual void FixedUpdate()
        {
            // noop
        }

        public virtual void OnExit()
        {
            // noop
        }
    }
}