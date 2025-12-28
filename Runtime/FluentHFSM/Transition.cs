using System;

namespace HashStudios.FluentHFSM
{
    internal sealed class Transition
    {
        public readonly State To;
        public readonly Func<bool> Condition;

        public Transition(State to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }
}