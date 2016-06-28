using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using UnityEditor;
using UnityEngine.UI;

namespace NodeEditorFramework.Standard {
    /**
     * Dialogue Node featuring a precheck per output.
     * This system is designed to take a unique path down the graph from a root node.
     * Multiple connections per output are getting ignored (by the interface) - so ignore them.
     */
    [System.Serializable]
    [Node(false, "Dialogue/Basic Dialogue")]
    public class DialogueNode : Node {
        public const string ID = "dialogueNode";

        public override string GetID {
            get { return ID; }
        }

        public override bool AllowRecursion {
            get { return true; }
        }

        private int extOptions {get { return Outputs.Count; } } // Totally should
        private int inOptions {get { return Inputs.Count; } }   // be removed.
        public string _dialogueText = "Dialogue Main Text";
        public List<string> _decisions = new List<string> {""};
        private Vector2 scroller;

        [SerializeField]
        private List<AbstractCheckable> _preCheckables = new List<AbstractCheckable> {null};

        public override Node Create(Vector2 pos) {
            DialogueNode node = CreateInstance<DialogueNode>();
            var max = Mathf.Max(inOptions, extOptions);
            node.rect = new Rect(pos.x, pos.y, 300, 200 + 50*max);
            node.name = "Dialogue Node";

            node.CreateInput("Parent", "Void");
            node.CreateOutput("Child", "Void");
            return node;
        }

        protected internal override void NodeGUI() {
            // super hard coded to get the node UI not completely broken.
            // I figured that the NodeGUI wasn't planned to do fancy things, so this is with fixed sizes everywhere.
            var max = Mathf.Max(inOptions, extOptions);
            scroller = GUILayout.BeginScrollView(scroller);
                // if we ever manage to break the UI again, then we have scrollbars.
            rect.height = 200 + 50*max; // we can calculate a fixed node size with a fixed calculation
            _dialogueText = GUILayout.TextArea(_dialogueText, GUILayout.Height(100)); // main dialogue

            for (int i = 0; i < max; i++) {
                // until max is reached we do...

                GUILayout.BeginHorizontal(); // new row to throw our pre check in
                GUILayout.BeginVertical();
                GUILayout.EndVertical();
                if (i < extOptions) {
                    _preCheckables[i] =
                        (AbstractCheckable)
                            EditorGUILayout.ObjectField(_preCheckables[i], typeof(AbstractCheckable), true);
                } else {
                    GUILayout.Label("");
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal(); // a horizontal
                GUILayout.BeginVertical(); // display parent
                if (i < inOptions) {
                    Inputs[i].DisplayLayout();
                }
                else {
                    GUILayout.Label("", GUILayout.Width(41)); // or an invisible label to not break the next coming box
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                if (i < extOptions) {
                    _decisions[i] = GUILayout.TextArea(_decisions[i], GUILayout.Height(30), GUILayout.Width(200));
                        // a two line text box
                }
                else {
                    GUILayout.Label("", GUILayout.Height(30)); // or just a magical label
                }
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                if (i < extOptions) {
                    Outputs[i].DisplayLayout(); // output - can be missing to work well, too.
                }
                GUILayout.EndVertical();

                GUILayout.EndHorizontal(); // end horizontal
            }

            GUILayout.FlexibleSpace(); // Fills out the bottom

            // node option panel
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Add Parent")) {
                CreateInput("Parent", "Void");
                NodeGUI();
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Add Child")) {
                CreateOutput("Child", "Void");
                _decisions.Add("");
                _preCheckables.Add(null);
                NodeGUI();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Remove Parent")) {
                DeleteInput(Inputs[inOptions - 1]);
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Remove Child")) {
                DeleteOutput(Outputs[extOptions - 1]);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Calculate")) {
                Debug.Log(CheckConstraintAt(1));
                Calculate();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
        }

        public override bool Calculate() {
            return allInputsReady();
        }

        /**
         * Returns false if:
         * - Checkable is false
         * Return true if:
         * - Checkable is null
         * - Checkable is true
         * Throws an IndexOutOfRangeException if index is out of bounds.
         */
        public bool CheckConstraintAt(int index) {
            Calculate();
            if (_preCheckables.Count <= index) {
                throw new IndexOutOfRangeException("Out of bounds: index at " + index);
            }
            if (_preCheckables[index] == null) {
                return true;
            }
            var boolean = _preCheckables[index].VariableCheck();
            Debug.Log(boolean);
            return boolean;
        }
    }
}
