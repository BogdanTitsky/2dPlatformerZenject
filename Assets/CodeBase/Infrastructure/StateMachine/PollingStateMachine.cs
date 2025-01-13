using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure.StateMachine
{
    public class PollingStateMachine
    {
        private StateNode current;
        private readonly Dictionary<Type, StateNode> nodes = new();
        private readonly HashSet<IPredicateTransition> anyTransitions = new();

        public void Update()
        {
            IPredicateTransition predicateTransition = GetTransition();
            if (predicateTransition != null)
                ChangeState(predicateTransition.To);

            current.State?.OnUpdate();
        }

        public void FixedUpdate()
        {
            current.State?.OnFixedUpdate();
        }

        public void SetState(IState state)
        {
            current?.State.OnExit();
            current = nodes[state.GetType()];
            current.State?.OnEnter();
        }

        public IState CurrentState() => current.State;

        private void ChangeState(IState state)
        {
            if (state == current.State) return;
            
            IState previousState = current.State;
            IState nextState = nodes[state.GetType()].State;

            previousState?.OnExit();
            nextState?.OnEnter();
            current = nodes[state.GetType()];
        }

        private IPredicateTransition GetTransition()
        {
            foreach (IPredicateTransition transition in anyTransitions)
                if (transition.Condition.Evaluate())
                    return transition;

            foreach (IPredicateTransition transition in current.Transitions)
                if (transition.Condition.Evaluate())
                    return transition;

            return null;
        }

        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition(IState to, IPredicate condition)
        {
            anyTransitions.Add(new PredicateTransition(GetOrAddNode(to).State, condition));
        }

        private StateNode GetOrAddNode(IState state)
        {
            StateNode node = nodes.GetValueOrDefault(state.GetType());

            if (node == null)
            {
                node = new StateNode(state);
                nodes.Add(state.GetType(), node);
            }

            return node;
        }

        private class StateNode
        {
            public IState State { get; }
            public HashSet<IPredicateTransition> Transitions { get; }
    
            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<IPredicateTransition>();
            }
    
            public void AddTransition(IState to, IPredicate condition)
            {
                Transitions.Add(new PredicateTransition(to, condition));
            }
        }
    }
}