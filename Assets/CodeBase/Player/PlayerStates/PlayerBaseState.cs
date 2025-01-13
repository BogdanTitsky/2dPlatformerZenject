using CodeBase.Infrastructure.StateMachine;

namespace CodeBase.Player.PlayerStates
{
    public abstract class PlayerBaseState : IState
    {
        protected readonly HeroAnimator animator;

        protected PlayerBaseState(HeroAnimator animator)
        {
            this.animator = animator;
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}