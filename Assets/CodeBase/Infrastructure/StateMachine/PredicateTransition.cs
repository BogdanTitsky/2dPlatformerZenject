namespace CodeBase.Infrastructure.StateMachine {
    public class PredicateTransition : IPredicateTransition {
        public IState To { get; }
        public IPredicate Condition { get; }

        public PredicateTransition(IState to, IPredicate condition) {
            To = to;
            Condition = condition;
        }
    }
}