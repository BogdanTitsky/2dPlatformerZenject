namespace CodeBase.Player.PlayerStates
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(HeroAnimator animator) : base(animator)
        {
        }

        public override void OnEnter()
        {
            animator.PlayIdle();
        }
    }
}