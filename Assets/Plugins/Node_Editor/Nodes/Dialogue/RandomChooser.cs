using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using UnityEditor;

[Node (false, "Dialogue/Random Choose")]
public class RandomChooser : Node {
	public const string ID = "randomChooser";
	public override string GetID{ get { return ID; } }
	public override bool AllowRecursion { get { return true; } }

	private string label = "Not Calculated yet.";

	public override Node Create(Vector2 pos) {
		RandomChooser node = CreateInstance<RandomChooser>();
		node.rect = new Rect(pos.x, pos.y, 200, 50 * 5);
		node.name = "Random Node";

		node.CreateInput("Parent", "Boolean");
		node.CreateOutput("Child_MAIN", "Boolean");
		node.CreateOutput("Child1", "Boolean");
		node.CreateOutput("Child2", "Boolean");
		node.CreateOutput("Child3", "Boolean");
		node.CreateOutput("Child4", "Boolean");
		node.CreateOutput("Child5", "Boolean");
		return node;
	}

	protected internal override void NodeGUI() {
		if (GUILayout.Button ("Calculate")) {
			Calculate ();
            EditorUtility.SetDirty(this);
        }
        rect.height = 50 * 5;
		GUILayout.Label (label);

		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		Inputs[0].DisplayLayout();
		GUILayout.EndVertical();

		GUILayout.BeginVertical();
		Outputs[0].DisplayLayout();
		GUILayout.EndVertical();

		GUILayout.EndHorizontal();
		for(int i = 1; i < 5; i++) {
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			Outputs[i].DisplayLayout();
			GUILayout.EndVertical();

			GUILayout.EndHorizontal();
		}
		GUILayout.BeginHorizontal ();
		GUILayout.BeginVertical ();

	}

	public override bool Calculate() {
		/*
		if(!allInputsReady()) {
			return false;
		}
		*/
		float rand = Random.value;
		label = "Selection: " + ((int) (rand / 0.2f) + 1);
		return true;
	}
}
