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

        private int extOptions = 1;
        private int inOptions = 1;
        public string _dialogueText = "Dialogue Main Text";
        public List<string> _decisions = new List<string> {""};
        private Vector2 scroller;
        /*
         * @TODO: This has currently no implied logic, pls add.
         */
        private List<AbstractCheckable> _preCheckables = new List<AbstractCheckable> {null};

        public override Node Create(Vector2 pos) {
            DialogueNode node = CreateInstance<DialogueNode>();
            var max = Mathf.Max(inOptions, extOptions);
            node.rect = new Rect(pos.x, pos.y, 300, 200 + 50*max);
            node.name = "Dialogue Node";

            node.CreateInput("Parent", "Boolean");
            node.CreateOutput("Child", "Boolean");
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
                inOptions++;
                CreateInput("Parent", "Boolean");
                EditorUtility.SetDirty(this);
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Add Child")) {
                extOptions++;
                CreateOutput("Child", "Boolean");
                _decisions.Add("");
                _preCheckables.Add(null);
                EditorUtility.SetDirty(this);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Remove Parent")) {
                inOptions--;
                Inputs.RemoveAt(inOptions);
                Inputs.TrimExcess();
                EditorUtility.SetDirty(this);
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Remove Child")) {
                extOptions--;
                Outputs.RemoveAt(extOptions);
                Outputs.TrimExcess();
                EditorUtility.SetDirty(this);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            if (GUILayout.Button("Calculate")) {
                Calculate();
                EditorUtility.SetDirty(this);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
        }

        public override bool Calculate() {
            for (int index = 0; index < _preCheckables.Count; index++) {
                AbstractCheckable checkable = _preCheckables[index];
                // If the pre check is not null, then set outgoing value on it
                if (checkable != null) {
                    Outputs[index].SetValue<bool>(checkable.VariableCheck());
                }
            }
            Debug.Log(_dialogueText);
            return allInputsReady();
        }
    }
}