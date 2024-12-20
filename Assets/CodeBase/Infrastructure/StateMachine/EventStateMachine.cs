using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure.StateMachine
{
    public class EventStateMachine
    {
        private StateNode current;
        private readonly Dictionary<Type, StateNode> nodes = new();
        private readonly HashSet<ITransition> anyTransitions = new();

        public IState CurrentState() => current.State;
        
        public void Update() =>
            current.State?.Update();

        public void FixedUpdate() =>
            current.State?.FixedUpdate();

        public void AddTransition(IState from, IState to) =>
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State);

        public void AddAnyTransition(IState to) =>
            anyTransitions.Add(new EventTransition(GetOrAddNode(to).State));

        public void SetInitialState(IState state)
        {
            current = nodes[state.GetType()];
            current.State?.OnEnter();
        }

        public void ChangeState(IState state)
        {
            if (state == current.State || !IsTransitionExist(state)) return;

            IState previousState = current.State;
            IState nextState = nodes[state.GetType()].State;

            previousState?.OnExit();
            nextState?.OnEnter();
            current = nodes[state.GetType()];
        }

        private bool IsTransitionExist(IState state)
        {
            foreach (ITransition transition in anyTransitions)
                if (transition.To == state)
                    return true;

            foreach (ITransition transition in current.Transitions)
                if (transition.To == state)
                    return true;
            return false;
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
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to) => 
                Transitions.Add(new EventTransition(to));
        }
    }
}