using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using UnityEditor;

namespace NodeEditorFramework.Standard {
    [System.Serializable]
    [Node(false, "Dialogue/Variable Checker")]
    public class VariableChecker : Node {
        public const string ID = "variableChecker";

        public override string GetID {
            get { return ID; }
        }

        public override bool AllowRecursion {
            get { return true; }
        }

        public Node selectedNode;

        [SerializeField]
        private AbstractCheckable _abstractCheckable;

        // Use this for initialization
        public override Node Create(Vector2 pos) {
            VariableChecker node = CreateInstance<VariableChecker>();
            node.rect = new Rect(pos.x, pos.y, 200, 80);

            node.name = "Varibale Check";
            node.CreateInput("Parent", "Void");
            node.CreateOutput("true", "Void");
            node.CreateOutput("false", "Void");
            return node;
        }

        // Update is called once per frame
        protected internal override void NodeGUI() {
            GUILayout.BeginHorizontal ();
            _abstractCheckable = (AbstractCheckable) EditorGUILayout.ObjectField(_abstractCheckable, typeof(AbstractCheckable), true);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
		    GUILayout.BeginVertical ();
            Inputs[0].DisplayLayout();
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            Outputs[0].DisplayLayout();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            Outputs[1].DisplayLayout();
            GUILayout.EndHorizontal();
        }

        public override bool Calculate() {
            var check = false;
            if (_abstractCheckable != null) {
                check = _abstractCheckable.VariableCheck();
                selectedNode = check ? Outputs[0].connections[0].body : Outputs[1].connections[0].body;
            }
            return check;
        }
    }
}