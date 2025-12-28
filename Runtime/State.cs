using System;
using System.Collections.Generic;

namespace HashStudios.FluentHFSM
{
    public class State : IState
    {
        private readonly List<Transition> transitions = new();
        private readonly List<State> subStates = new();

        public State Parent;
        private State activeSubState;

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void Tick(float deltaTime) { }

#if UNITY_EDITOR
        public IReadOnlyList<State> SubStates => subStates;
        public State ActiveSubState => activeSubState;
        public string DebugName => GetType().Name;
#endif
        public void AddSubState(State subState)
        {
            subState.Parent = this;
            subStates.Add(subState);
        }

        public void SetInitialSubState(State state)
        {
            activeSubState = state;
        }

        public void AddTransition(State to, Func<bool> condition)
        {
            transitions.Add(new Transition(to, condition));
        }

        internal Transition CheckTransitions()
        {
            foreach (var transition in transitions)
            {
                if (transition.Condition())
                    return transition;
            }

            return null;
        }

        internal void Enter()
        {
            OnEnter();
            activeSubState?.Enter();
        }

        internal void Exit()
        {
            activeSubState?.Exit();
            OnExit();
        }

        internal void Update(float deltaTime)
        {
            Tick(deltaTime);
            activeSubState?.Update(deltaTime);
        }

        internal State GetActiveLeaf()
        {
            return activeSubState == null
                ? this
                : activeSubState.GetActiveLeaf();
        }

        internal void SwitchSubState(State next)
        {
            activeSubState = next;
            activeSubState.Enter();
        }

    }
}