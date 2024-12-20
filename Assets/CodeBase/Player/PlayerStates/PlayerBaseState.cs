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

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}