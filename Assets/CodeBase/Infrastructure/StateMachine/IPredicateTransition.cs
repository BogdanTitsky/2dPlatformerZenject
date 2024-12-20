namespace CodeBase.Infrastructure.StateMachine {
    public interface IPredicateTransition : ITransition {
        IPredicate Condition { get; }
    }
    
    public interface ITransition
    {
        IState To { get; }
    }
}