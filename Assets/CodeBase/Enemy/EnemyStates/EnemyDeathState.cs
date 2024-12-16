namespace CodeBase.Enemy.EnemyStates
{
    public class EnemyDeathState : EnemyBaseState
    {
        private EnemyMoveToPlayer move;
        private readonly EnemyStateMachine enemyStateMachine;

        public EnemyDeathState(EnemyAnimator animator, EnemyMoveToPlayer move, EnemyStateMachine enemyStateMachine) : base(animator)
        {
            this.move = move;
            this.enemyStateMachine = enemyStateMachine;
        }

        public override void OnEnter()
        {
            animator.PlayDeath();
            move.StopMove();
            enemyStateMachine.DestroyEnemy();
        }
    }
}