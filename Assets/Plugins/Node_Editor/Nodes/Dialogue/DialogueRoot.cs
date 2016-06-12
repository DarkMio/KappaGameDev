using UnityEngine;
using System.Collections;

namespace NodeEditorFramework.Standard {
    [System.Serializable]
    [Node(false, "Dialogue/Dialogue Root")]
    public class DialogueRoot : Node {
        public const string ID = "rootNode";

        public override string GetID {
            get { return ID; }
        }

        public Node firstNode;

        public override Node Create(Vector2 pos) {
            DialogueRoot node = CreateInstance<DialogueRoot>();
            node.rect = new Rect(pos.x, pos.y, 100, 40);
            node.name = "Dialogue Root";
            node.CreateOutput("Root", "Void");
            return node;
        }

        protected internal override void NodeGUI() {
            Outputs[0].DisplayLayout();
        }

        public override bool Calculate() {
            firstNode = Outputs[0].connections[0].body;
            return allInputsReady();
        }

        public override bool AllowRecursion {
            get { return false; }
        }
    }
}