namespace CodeBase.Infrastructure.GameStates
{
  public interface IGameState: IExitableState
  {
    void Enter();
  }

  public interface IPayloadedState<TPayload> : IExitableState
  {
    void Enter(TPayload payload);
  }
  
  public interface IExitableState
  {
    void Exit();
  }
}