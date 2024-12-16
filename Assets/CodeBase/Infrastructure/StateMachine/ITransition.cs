namespace CodeBase.Infrastructure.StateMachine {
    public interface ITransition {
        IState To { get; }
        IPredicate Condition { get; }
    }
}