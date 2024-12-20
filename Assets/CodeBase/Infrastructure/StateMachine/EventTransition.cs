namespace CodeBase.Infrastructure.StateMachine
{
    public class EventTransition : ITransition
    {
        public IState To { get; }

        public EventTransition(IState to)
        {
            To = to;
        }
    }
}