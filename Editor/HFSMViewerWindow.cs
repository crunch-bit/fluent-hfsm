#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HashStudios.FluentHFSM.Editor
{
    public class HFSMViewerWindow : EditorWindow
    {
        private int selectedIndex;
        private HierarchicalStateMachine currentMachine;

        [MenuItem("Window/HFSM/State Machine Viewer")]
        public static void Open()
        {
            GetWindow<HFSMViewerWindow>("HFSM Viewer");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("HFSM Debug Viewer", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            var machines = HFSMDebugRegistry.Machines;

            if (machines.Count == 0)
            {
                EditorGUILayout.HelpBox("No active HFSM.", MessageType.Info);
                return;
            }
            selectedIndex = Mathf.Clamp(selectedIndex, 0, machines.Count - 1);

            if (machines.Count > 1)
            {
                selectedIndex = EditorGUILayout.Popup(
                    "State Machine",
                    selectedIndex,
                    GetMachineNames(machines)
                );
            }

            currentMachine = machines[selectedIndex];
            DrawState(currentMachine.DebugRoot, 0);

            Repaint();
        }

        private string[] GetMachineNames(IReadOnlyList<HierarchicalStateMachine> machines)
        {
            var names = new string[machines.Count];
            for (int i = 0; i < machines.Count; i++)
                names[i] = $"HFSM {i}";

            return names;
        }

        private void DrawState(State state, int indent)
        {
            if (state == null)
                return;

            bool isActive = IsActive(state);

            var style = new GUIStyle(EditorStyles.label);
            if (isActive)
            {
                style.fontStyle = FontStyle.Bold;
                style.normal.textColor = Color.green;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(indent * 16);
            EditorGUILayout.LabelField(
                isActive ? $"▶ {state.DebugName}" : state.DebugName,
                style
            );
            EditorGUILayout.EndHorizontal();

            foreach (var sub in state.SubStates)
            {
                DrawState(sub, indent + 1);
            }
        }

        private bool IsActive(State state)
        {
            var current = currentMachine.DebugRoot;

            while (current != null)
            {
                if (current == state)
                    return true;

                current = current.ActiveSubState;
            }

            return false;
        }
    } 
}
#endif
