using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class PauseGameState : IState
    {
        public PauseGameState(IGameStateMachine stateMachine)
        {
        }

        public void Exit()
        {
            Time.timeScale = 1;
        }

        public void Enter()
        {
            Time.timeScale = 0;
        }
        
            
    }
}