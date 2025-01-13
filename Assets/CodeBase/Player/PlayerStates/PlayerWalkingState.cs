using UnityEngine;

namespace CodeBase.Player.PlayerStates
{
    public class PlayerWalkingState : PlayerBaseState
    {
        public Vector2 MoveDirection;
        
        private HeroMove _heroMove;
        public PlayerWalkingState(HeroAnimator animator, HeroMove heroMove) : base(animator)
        {
            _heroMove = heroMove;
        }

        public override void OnEnter()
        {
            animator.PlayWalking();
        }

        public override void OnUpdate()
        {
            _heroMove.Move(MoveDirection);            

        }

        public override void OnFixedUpdate()
        {
            _heroMove.ApplyMove();
        }

        public override void OnExit()
        {
            _heroMove.MoveOff();
        }
    }
}