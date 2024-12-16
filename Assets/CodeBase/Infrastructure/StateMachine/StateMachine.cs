﻿using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure.StateMachine
{
    public class StateMachine
    {
        private StateNode current;
        private readonly Dictionary<Type, StateNode> nodes = new();
        private readonly HashSet<ITransition> anyTransitions = new();

        public void Update()
        {
            ITransition transition = GetTransition();
            if (transition != null)
                ChangeState(transition.To);

            current.State?.Update();
        }

        public void FixedUpdate()
        {
            current.State?.FixedUpdate();
        }

        public void SetState(IState state)
        {
            current?.State.OnExit();
            current = nodes[state.GetType()];
            current.State?.OnEnter();
        }

        private void ChangeState(IState state)
        {
            if (state == current.State) return;

            IState previousState = current.State;
            IState nextState = nodes[state.GetType()].State;

            previousState?.OnExit();
            nextState?.OnEnter();
            current = nodes[state.GetType()];
        }

        private ITransition GetTransition()
        {
            foreach (ITransition transition in anyTransitions)
                if (transition.Condition.Evaluate())
                    return transition;

            foreach (ITransition transition in current.Transitions)
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
            anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
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

            public void AddTransition(IState to, IPredicate condition)
            {
                Transitions.Add(new Transition(to, condition));
            }
        }
    }
}