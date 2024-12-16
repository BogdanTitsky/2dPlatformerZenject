using UnityEngine;

namespace CodeBase.Enemy.EnemyStates
{
    public class EnemyChaseState : EnemyBaseState
    {
        private readonly EnemyMoveToPlayer moveToPlayer;

        public EnemyChaseState(EnemyMoveToPlayer enemyMoveToPlayer, EnemyAnimator animator) : base(animator)
        {
            moveToPlayer = enemyMoveToPlayer;
        }

        public override void OnEnter()
        {
            Debug.Log("ChaseState");
            animator.Move();
        }

        public override void OnExit()
        {
            moveToPlayer.StopMove();
        }

        public override void Update()
        {
            moveToPlayer.Chase();
        }
    }
}