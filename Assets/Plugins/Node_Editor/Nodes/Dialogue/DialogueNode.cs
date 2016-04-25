using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

[Node (false, "Dialogue/Basic Dialogue")]
public class DialogueNode : Node {
	public const string ID = "dialogueNode";
	public override string GetID{ get { return ID; } }
	public override bool AllowRecursion { get { return true; } }

	private int extOptions = 1;
	private int inOptions = 1;
	
	public override Node Create(Vector2 pos) {
		DialogueNode node = CreateInstance<DialogueNode>();
		node.rect = new Rect(pos.x, pos.y, 200, 100 + 50 * extOptions);
		node.name = "Dialogue Node";
		
		node.CreateInput("Parent", "Boolean");
		node.CreateOutput("Child", "Boolean");
		return node;
	}
	
	protected internal override void NodeGUI() {
		int max = Mathf.Max(inOptions, extOptions);
		rect.height = 63 + 18 * max;
		
		GUILayout.Label("Dialogue Node checking in!");
		
		for(int i = 0; i < max; i++) {
			GUILayout.BeginHorizontal();
			if(i < inOptions) {
				GUILayout.BeginVertical();
				Inputs[i].DisplayLayout();
				GUILayout.EndVertical();
			}
			if(i < extOptions) {
				GUILayout.BeginVertical();
				Outputs[i].DisplayLayout();
				GUILayout.EndVertical();
			}

			GUILayout.EndHorizontal();
		}
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		if(GUILayout.Button("Add Parent")) {
			inOptions++;
			CreateInput("Parent", "Boolean");
		}

		GUILayout.EndVertical();
		GUILayout.BeginVertical();
		if(GUILayout.Button("Add Child")) {
			extOptions++;
			CreateOutput("Child", "Boolean");
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}
	
	public override bool Calculate() {
		if(!allInputsReady()) {
			return false;
		}
		Outputs[0].SetValue<float>(Inputs[0].GetValue<float>() * 5);
		return true;
	}
}
