using System;
using System.Collections.Generic;

namespace HashStudios.FluentHFSM
{
    public static class StateFluentExtensions
    {
        private static readonly Dictionary<State, TransitionBuilder> builders = new();

        public static State When(this State from, Func<bool> condition)
        {
            builders[from] = new TransitionBuilder { Condition = condition };
            return from;
        }

        public static void GoTo(this State from, State to)
        {
            if (!builders.TryGetValue(from, out var builder))
                throw new InvalidOperationException("GoTo called without When.");

            from.AddTransition(to, builder.Condition);
            builders.Remove(from);
        }
    }

    internal sealed class TransitionBuilder
    {
        public Func<bool> Condition;
    }
}