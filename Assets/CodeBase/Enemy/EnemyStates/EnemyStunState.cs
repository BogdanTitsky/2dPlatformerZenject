using UnityEngine;

namespace CodeBase.Enemy.EnemyStates
{
    public class EnemyStunState : EnemyBaseState
    {
        public EnemyStunState(EnemyAnimator animator) : base(animator)
        {
        }

        private float stunDuration = 1;
        private float elapsedTime = 0;
        private bool stunned = false;
        public override void OnEnter()
        {
            animator.PlayStun();
            elapsedTime = 0;
        }

        public override void OnExit()
        {
            stunned = false;
        }

        public override void OnUpdate()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= stunDuration)
            {
                stunned = true;
            }
        }
        
        
        public void SetStunDuration(float duration) => stunDuration = duration;

        public bool StunOver() => stunned;
    }
}