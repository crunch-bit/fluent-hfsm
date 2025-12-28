#if UNITY_EDITOR
using System.Collections.Generic;

namespace HashStudios.FluentHFSM
{
    public static class HFSMDebugRegistry
    {
        private static readonly List<HierarchicalStateMachine> machines = new();

        public static IReadOnlyList<HierarchicalStateMachine> Machines => machines;

        public static void Register(HierarchicalStateMachine machine)
        {
            if (!machines.Contains(machine))
                machines.Add(machine);
        }

        public static void Unregister(HierarchicalStateMachine machine)
        {
            machines.Remove(machine);
        }
    }
}
#endif
