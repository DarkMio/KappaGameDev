using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

namespace NodeEditorFramework.Standard {
    [System.Serializable]
    [Node (false, "Dialogue/Random Branch")]
    public class RandomBranchNode : Node 
    {
        public const string ID = "randomBranchNode";
        public override string GetID { get { return ID; } }

        public override Node Create (Vector2 pos) 
        {
            RandomBranchNode node = CreateInstance<RandomBranchNode> ();

            node.rect = new Rect (pos.x, pos.y, 100, 120);
            node.name = "RND Branch";

            node.CreateInput ("Source", "Void");

            node.CreateOutput ("Branch", "Void");
            node.CreateOutput ("Branch", "Void");
            node.CreateOutput ("Branch", "Void");
            node.CreateOutput ("Branch", "Void");

            return node;
        }

        protected internal override void NodeGUI () 
        {
            Inputs[0].DisplayLayout ();

            foreach (NodeOutput output in Outputs)
                output.DisplayLayout ();
        }

        public override bool Calculate () 
        {
            List<NodeOutput> connectedOutputs = new List<NodeOutput> ();
            foreach (NodeOutput output in Outputs)
            { // Get all connected outputs to take into consideration
                output.calculationBlockade = true;
                if (output.connections.Count > 0)
                    connectedOutputs.Add (output);
            }

            if (connectedOutputs.Count > 0)
            { // Select a random branch from the connected ones and unblock it so it gets calculated
                int randomSelected = Random.Range (0, connectedOutputs.Count);
                NodeOutput selectedOutput = connectedOutputs[randomSelected];
                selectedOutput.calculationBlockade = false;
                // Just pass the previous value to the selected branch
                selectedOutput.SetValue (Inputs[0].GetValue ());
            }
            return true;
        }
    }
}