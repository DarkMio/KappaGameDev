using UnityEngine;
using System.Collections;
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


        private AbstractCheckable _abstractCheckable;

        // Use this for initialization
        public override Node Create(Vector2 pos) {
            VariableChecker node = CreateInstance<VariableChecker>();
            node.rect = new Rect(pos.x, pos.y, 200, 60);

            node.name = "Varibale Check";
            node.CreateInput("Parent", "Boolean");
            node.CreateOutput("Child", "Boolean");
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
        }

        public override bool Calculate() {
            return true;
        }
    }
}