using UnityEngine;

namespace CodeBase.Enemy.EnemyStates
{
    public class EnemyAttackState : EnemyBaseState
    {
        private readonly Transform player;
        private readonly EnemyAttackBehaviour attackBehaviour;
        private readonly EnemyMoveToPlayer moveToPlayer;

        public EnemyAttackState(EnemyAttackBehaviour enemyAttackBehaviour,EnemyMoveToPlayer moveToPlayer, EnemyAnimator animator) : base(animator)
        {
            attackBehaviour = enemyAttackBehaviour;
            this.moveToPlayer = moveToPlayer;
        }

        public override void OnEnter()
        {            
            Debug.Log("AttackState");
        }

        public override void OnUpdate()
        {
            moveToPlayer.LookAtTarget();
            attackBehaviour.OnUpdate();
        }
    }
}