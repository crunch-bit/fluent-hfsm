using System.Collections.Generic;

namespace HashStudios.FluentHFSM
{
    public sealed class HierarchicalStateMachine
    {
        private readonly State root;

#if UNITY_EDITOR
        public State DebugRoot => root;
#endif

        public HierarchicalStateMachine(State root)
        {
            this.root = root;

#if UNITY_EDITOR
            HFSMDebugRegistry.Register(this);
#endif
        }
        public void Initialize()
        {
            root.Enter();
        }

        public void Update(float deltaTime)
        {
            var leaf = root.GetActiveLeaf();

            var transition = ResolveTransition(leaf);
            if (transition != null)
            {
                PerformTransition(leaf, transition.To);
                return;
            }

            root.Update(deltaTime);
        }

        public void Stop()
        {
            root.Exit();

#if UNITY_EDITOR
            HFSMDebugRegistry.Unregister(this);
#endif
        }

        private Transition ResolveTransition(State state)
        {
            while (state != null)
            {
                var transition = state.CheckTransitions();
                if (transition != null)
                    return transition;

                state = state.Parent;
            }

            return null;
        }

        private void PerformTransition(State from, State to)
        {
            var commonParent = FindCommonParent(from, to);

            ExitUpTo(from, commonParent);
            EnterDownFrom(commonParent, to);
        }

        private void ExitUpTo(State from, State until)
        {
            var state = from;

            while (state != until)
            {
                state.Exit();
                state = state.Parent;
            }
        }
        private void EnterDownFrom(State parent, State target)
        {
            var path = new Stack<State>();

            var state = target;
            while (state.Parent != parent)
            {
                path.Push(state);
                state = state.Parent;
            }

            path.Push(state);

            var current = parent;

            while (path.Count > 0)
            {
                var next = path.Pop();
                current.SwitchSubState(next);
                current = next;
            }
        }

        private static State FindCommonParent(State a, State b)
        {
            var ancestors = new HashSet<State>();

            while (a != null)
            {
                ancestors.Add(a);
                a = a.Parent;
            }

            while (b != null)
            {
                if (ancestors.Contains(b))
                    return b;

                b = b.Parent;
            }

            return null;
        }
    }
}